using Derify.Core.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Derify.Core.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private readonly string _connectionString;
        public BaseRepository(string connectionString) 
        {
            _connectionString = connectionString;
        }
        public List<RelationEntity> GetAll()
        {
            string query = @"
SELECT 
	distinct
	s.name [NombreSchema],
    t.name AS [NombreTabla],
	'[' +s.Name + '].[' + t.name + ']' [NombreTablaCompleto],
	c.column_id [IdCampo],
    c.name AS [NombreCampo], 
    case 
		when trim(lower(ty.name)) = 'varchar' then 'varchar(' + iif(c.max_length <= 0,'max',convert(varchar(50),c.max_length)) + ')'
		when trim(lower(ty.name)) = 'nvarchar' then 'nvarchar(' + iif(c.max_length <= 0,'max',convert(varchar(50),c.max_length / 2)) + ')'
		when trim(lower(ty.name)) = 'decimal' then 'decimal(' + convert(varchar(50),c.precision) + ',' +convert(varchar(50),c.scale) + ')'
		when trim(lower(ty.name)) = 'numeric' then 'numeric(' + convert(varchar(50),c.precision) + ',' +convert(varchar(50),c.scale) + ')'
		else
			ty.name
	end AS [TipoDatoCampo],
    CASE 
        WHEN ic.column_id IS NOT NULL THEN 1
        ELSE 0
    END AS [EsPK],
    CASE 
        WHEN iu.column_id IS NOT NULL THEN 1
        ELSE 0
    END AS [EsUnico],
    CASE 
        WHEN fk.name IS NOT NULL THEN 1
        ELSE 0
    END AS [EsFK],
    OBJECT_NAME(fk.referenced_object_id) AS [ReferenciadoPor],
    CASE 
        WHEN c.is_nullable = 1 THEN 'null'
        ELSE ''
    END AS [Nulleable],
    replace(replace(dc.definition,'(',''),')','') AS [Default]
FROM sys.tables t
INNER JOIN sys.columns c ON c.object_id = t.object_id
INNER JOIN sys.types ty ON ty.user_type_id = c.user_type_id
LEFT JOIN sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id AND ic.index_id = 1
LEFT JOIN sys.index_columns iu ON iu.object_id = c.object_id AND iu.column_id = c.column_id AND iu.index_id > 1
LEFT JOIN sys.foreign_key_columns fkc ON fkc.parent_object_id = t.object_id AND fkc.parent_column_id = c.column_id
LEFT JOIN sys.foreign_keys fk ON fk.object_id = fkc.constraint_object_id
LEFT JOIN sys.default_constraints dc ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
INNER JOIN sys.schemas s on s.schema_id = t.schema_id
order by 1,2;
            ";

            DataTable dt = new DataTable();

            using(SqlConnection connection = new SqlConnection(_connectionString)) 
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;

                    using(SqlDataAdapter adapter = new SqlDataAdapter(command)) 
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            List<RelationEntity> relationEntities = new List<RelationEntity>();

            foreach(DataRow dr in dt.Rows) 
            {
                relationEntities.Add(new RelationEntity()
                {
                    SchemaName = Convert.ToString(dr["NombreSchema"]) ?? string.Empty,
                    TableName = Convert.ToString(dr["NombreTabla"]) ?? string.Empty,
                    FullTableName = Convert.ToString(dr["NombreTablaCompleto"]) ?? string.Empty,
                    FieldName = Convert.ToString(dr["NombreCampo"]) ?? string.Empty,
                    FieldDataType = Convert.ToString(dr["TipoDatoCampo"]) ?? string.Empty,
                    IsPrimaryKey = Convert.ToBoolean(dr["EsPK"]),
                    IsUnique = Convert.ToBoolean(dr["EsUnico"]),
                    IsForeignKey = Convert.ToBoolean(dr["EsFK"]),
                    ReferencedBy = Convert.ToString(dr["ReferenciadoPor"]) ?? string.Empty,
                    Nulleable = Convert.ToString(dr["Nulleable"]) ?? string.Empty,
                    Default = Convert.ToString(dr["Default"]) ?? string.Empty
                }) ;
                
            }

            return relationEntities;
        }
    }
}
