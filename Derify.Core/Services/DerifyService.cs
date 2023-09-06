using Derify.Core.Entities;
using Derify.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derify.Core.Services
{
    public class DerifyService : IDerifyService
    {
        private readonly IBaseRepository _baseRepository;
        public DerifyService(IBaseRepository repository) 
        {
            _baseRepository = repository;
        }

        public string GetMermaidCode() 
        {
            StringBuilder mermaid = new StringBuilder();

            try
            {
                List<RelationEntity> relationEntities = _baseRepository.GetAll();

                mermaid.AppendLine("erDiagram");

                IEnumerable<string> tableNames = relationEntities.Select(re => re.TableName).Distinct();
                
                //Creo las entidades
                foreach (string tableName in tableNames) 
                {
                    mermaid.AppendLine($"   \"{tableName}\" {{");
                    
                    IEnumerable<string> columns = relationEntities.Where(re => re.TableName == tableName).Select(re=> {
                        List<string> constraints = new List<string>();
                        if (re.IsPrimaryKey) constraints.Add("PK");
                        if (re.IsForeignKey) constraints.Add("FK");
                        if (re.IsUnique) constraints.Add("UK");

                        string constraint = string.Join(",", constraints);

                        return $"       {re.FieldName} {re.FieldDataType} {constraint}";
                    });
                    
                    foreach (string column in columns) 
                        mermaid.AppendLine(column);


                    mermaid.AppendLine("    }");
                }

                //Conecto las entidades
                foreach (string tableName in tableNames)
                {
                    IEnumerable<string> relations = relationEntities.Where(re => re.TableName == tableName && !string.IsNullOrEmpty(re.ReferencedBy)).Select(re => {
                        return $"   \"{re.ReferencedBy}\" ||--o{{ \"{tableName}\" : \"\"";
                    });

                    foreach (string relation in relations)
                        mermaid.AppendLine(relation);
                }
            }
            catch (Exception ex) 
            {
                mermaid.AppendLine(ex.Message);
            }

            return mermaid.ToString();
        }
    }
}
