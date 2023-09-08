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

        public string GetCode()
        {
            StringBuilder code = new StringBuilder();

            List<RelationEntity> relationEntities = _baseRepository.GetAll();
            IEnumerable<string> tableNames = relationEntities.Select(re => re.TableName).Distinct();
            
            int tableCount = 0;
            int topPos= 100;
            foreach (string tableName in tableNames)
            {
                IEnumerable<string> columns = relationEntities.Where(re => re.TableName == tableName).Select(re => {
                    List<string> constraints = new List<string>();
                    if (re.IsPrimaryKey) constraints.Add("PK");
                    if (re.IsForeignKey) constraints.Add("FK");
                    if (re.IsUnique) constraints.Add("UK");

                    string constraint = string.Join(",", constraints);

                    return $@"
                                <tr>
                                    <td class=""gray"">{constraint}</td>
                                    <td>{re.FieldName}</td>
                                    <td class=""accent"">{re.FieldDataType}</td>
                                    <td>{re.Nulleable}</td>
                                </tr>
                    ";
                });

                string startEntity = $@"
                    <div class=""card"" id=""{tableName}"" style=""left: {(tableCount * 400)+100}px; top: {topPos}px;"">
                        <div class=""card--title"">
                            {tableName}
                        </div>
                        <div class=""card--body"">
                            <table>
                                <tr>
                                    <th>Key</th>
                                    <th>Property</th>
                                    <th>Type</th>
                                    <th>Nullable</th>
                                </tr>
                ";

                code.AppendLine(startEntity);

                foreach (string column in columns)
                    code.AppendLine(column);

                string endEntity = $@"
                            </table>
                        </div>
                    </div>
                ";
                code.AppendLine(endEntity);


                if (tableCount < 3)
                {
                    tableCount++;
                }
                else
                {
                    tableCount = 0;
                    topPos += 400;
                }
            }

            //Conecto las entidades
            foreach (string tableName in tableNames)
            {
                IEnumerable<string> relations = relationEntities.Where(re => re.TableName == tableName && !string.IsNullOrEmpty(re.ReferencedBy)).Select(re => {
                    return $"<div source=\"{re.ReferencedBy}\" target=\"{tableName}\" class=\"link\"></div>";
                });

                foreach (string relation in relations)
                    code.AppendLine(relation);
            }

            return code.ToString();
        }

        
    }
}
