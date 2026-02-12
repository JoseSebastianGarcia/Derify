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
SELECT DISTINCT
    s.name                               AS [NombreSchema],
    t.name                               AS [NombreTabla],
    s.name + '.' + t.name                AS [NombreTablaCompleto],

    c.column_id                          AS [IdCampo],
    c.name                               AS [NombreCampo], 

    CASE 
        WHEN LOWER(TRIM(ty.name)) = 'varchar'  
            THEN 'varchar(' + IIF(c.max_length <= 0,'max',CONVERT(varchar(50),c.max_length)) + ')'
        WHEN LOWER(TRIM(ty.name)) = 'nvarchar' 
            THEN 'nvarchar(' + IIF(c.max_length <= 0,'max',CONVERT(varchar(50),c.max_length / 2)) + ')'
        WHEN LOWER(TRIM(ty.name)) = 'decimal'  
            THEN 'decimal(' + CONVERT(varchar(50),c.precision) + ',' + CONVERT(varchar(50),c.scale) + ')'
        WHEN LOWER(TRIM(ty.name)) = 'numeric'  
            THEN 'numeric(' + CONVERT(varchar(50),c.precision) + ',' + CONVERT(varchar(50),c.scale) + ')'
        ELSE ty.name
    END                                  AS [TipoDatoCampo],

    CASE WHEN ic.column_id IS NOT NULL THEN 1 ELSE 0 END AS [EsPK],
    CASE WHEN iu.column_id IS NOT NULL THEN 1 ELSE 0 END AS [EsUnico],
    CASE WHEN fk.name IS NOT NULL THEN 1 ELSE 0 END      AS [EsFK],

    c.is_identity                        AS [EsAutoincremental],
    c.is_nullable                        AS [Nulleable],

    REPLACE(REPLACE(dc.definition,'(',''),')','') AS [Default],

    rs.name [EsquemaReferenciado],
    rt.name [TablaReferenciada],
    rc.name [CampoReferenciado],
    rs.name + '.' +  rt.name AS [TablaReferenciadaCompleta],
    rs.name + '.' + rt.name + '.' + rc.name [CampoReferenciadoCompleto]

FROM sys.tables t
INNER JOIN sys.schemas s 
    ON s.schema_id = t.schema_id

INNER JOIN sys.columns c 
    ON c.object_id = t.object_id

INNER JOIN sys.types ty 
    ON ty.user_type_id = c.user_type_id

LEFT JOIN sys.index_columns ic 
    ON ic.object_id = c.object_id 
   AND ic.column_id = c.column_id 
   AND ic.index_id = 1

LEFT JOIN sys.index_columns iu 
    ON iu.object_id = c.object_id 
   AND iu.column_id = c.column_id 
   AND iu.index_id > 1

LEFT JOIN sys.foreign_key_columns fkc 
    ON fkc.parent_object_id = t.object_id 
   AND fkc.parent_column_id = c.column_id

LEFT JOIN sys.foreign_keys fk 
    ON fk.object_id = fkc.constraint_object_id

LEFT JOIN sys.tables rt
    ON rt.object_id = fkc.referenced_object_id

LEFT JOIN sys.schemas rs
    ON rs.schema_id = rt.schema_id

LEFT JOIN sys.columns rc
    ON rc.object_id = fkc.referenced_object_id
   AND rc.column_id = fkc.referenced_column_id

LEFT JOIN sys.default_constraints dc 
    ON dc.parent_object_id = c.object_id 
   AND dc.parent_column_id = c.column_id

ORDER BY 1,2;

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
					IsAutoincrement = Convert.ToBoolean(dr["EsAutoincremental"]),
					ReferencedSchema = Convert.ToString(dr["EsquemaReferenciado"]),
					ReferencedTable = Convert.ToString(dr["TablaReferenciada"]),
					ReferencedField = Convert.ToString(dr["CampoReferenciado"]),
					FullReferencedField = Convert.ToString(dr["CampoReferenciadoCompleto"]),
					FullReferencedTable = Convert.ToString(dr["TablaReferenciadaCompleta"]),
					Nulleable = Convert.ToBoolean(dr["Nulleable"]),
                    Default = Convert.ToString(dr["Default"]) ?? string.Empty
                }) ;
                
            }

            return relationEntities;
        }
    }
}
