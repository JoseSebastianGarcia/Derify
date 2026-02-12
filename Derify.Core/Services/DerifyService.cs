using Derify.Core.Entities;
using Derify.Core.Reponse;
using Derify.Core.Repository;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Derify.Core.Services;

public class DerifyService : IDerifyService
{
    private readonly IBaseRepository _baseRepository;
    public DerifyService(IBaseRepository repository) => _baseRepository = repository;
    
    public Result<GetDatabaseSchemaResponse> GetDatabaseSchema()
	{
        try
        {
            List<RelationEntity> relationEntities = _baseRepository.GetAll();

            GetDatabaseSchemaResponse getDatabaseSchemaResponse = new GetDatabaseSchemaResponse();


            relationEntities.GroupBy(re => re.SchemaName).ToList().ForEach(g =>
            {
                Schema schema = new Schema() { Name = g.Key, Tables = new List<Table>() };
                g.GroupBy(r => r.TableName).ToList().ForEach(t =>
                {
                    Table table = new Table() { Id = t.First().FullTableName, Name = t.First().TableName, Schema = t.First().SchemaName, Columns = new List<Column>() };
                    t.ToList().ForEach(f =>
                    {
                        Column field = new Column()
                        {
                            Name = f.FieldName,
                            Type = f.FieldDataType,
                            IsPK = f.IsPrimaryKey,
                            IsFK = f.IsForeignKey,
                            IsUK = f.IsUnique,
                            IsAutoIncrement = f.IsAutoincrement,
                            Nullable = f.Nulleable,
                            DefaultValue = f.Default,
                            FkReferencesColumn = string.IsNullOrEmpty(f.ReferencedField) ? null : f.ReferencedField,
                            FkReferencesTable = string.IsNullOrEmpty(f.FullReferencedTable) ? null : f.FullReferencedTable
                        };
                        table.Columns.Add(field);
                    });
                    schema.Tables.Add(table);
                });
                getDatabaseSchemaResponse.Schemas.Add(schema);
            });

            //And now relations
            relationEntities.Where(re => re.IsForeignKey).ToList().ForEach(relation =>
            {
                getDatabaseSchemaResponse.Relationships.Add(new Relationship()
                {
                    Id = $"FK_{relation.TableName}_{relation.FieldName}_{relation.ReferencedTable ?? ""}_{relation.ReferencedField ?? ""}",
                    SourceTable = relation.FullTableName,
                    SourceColumn = relation.FieldName,
                    TargetTable = relation.FullReferencedTable ?? "",
                    TargetColumn = relation.ReferencedField ?? "",
                    Cardinality = "N:1"
                });
            });

            return Result<GetDatabaseSchemaResponse>.Success(getDatabaseSchemaResponse,"Database schema retrieved successfully!");
        }
        catch(Exception ex)
        {
            return Result<GetDatabaseSchemaResponse>.Failure("An error occurred while retrieving the database schema", ex, "An unhandled error occurred!");
        }
	}
}
