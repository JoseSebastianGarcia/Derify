﻿using System;
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
        public string? ReferencedBy { get; set; }
        public string Nulleable { get; set; } = string.Empty;
        public string Default { get; set; } = string.Empty;
    }
}
