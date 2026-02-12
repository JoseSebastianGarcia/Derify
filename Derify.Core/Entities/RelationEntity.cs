using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derify.Core.Entities
{
    public class RelationEntity
    {
        public string SchemaName { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public string FullTableName { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public string FieldDataType { get; set; } = string.Empty;
        public bool IsPrimaryKey { set; get; }
        public bool IsUnique { set; get; }
        public bool IsForeignKey { set; get; }
        public bool IsAutoincrement { set; get; }
        public string? ReferencedSchema { get; set; }
		public string? ReferencedTable { get; set; }
		public string? ReferencedField { get; set; }
		public string? FullReferencedTable { get; set; }
		public string? FullReferencedField { get; set; }
		public bool Nulleable { get; set; }
        public string Default { get; set; } = string.Empty;
    }
}
