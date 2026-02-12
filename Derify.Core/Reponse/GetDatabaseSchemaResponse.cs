using System.Text.Json.Serialization;
using System.ComponentModel;
namespace Derify.Core.Reponse;

public class GetDatabaseSchemaResponse
{
	[JsonPropertyName("schemas")]
	public List<Schema> Schemas { get; set; } = [];

	[JsonPropertyName("relationships")]
	public List<Relationship> Relationships { get; set; } = [];
}

public class Schema
{
	[JsonPropertyName("name")]
	public string Name { get; set; } = "";

	[JsonPropertyName("tables")]
	public List<Table> Tables { get; set; } = [];
}

public class Table
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = "";
	[JsonPropertyName("schema")]
	public string Schema { get; set; } = "";
	[JsonPropertyName("name")]
	public string Name { get; set; } = "";
	[JsonPropertyName("columns")]
	public List<Column> Columns { get; set; } = [];
}

public class Column
{
	[JsonPropertyName("name")]
	public string Name { get; set; } = "";
	[JsonPropertyName("type")]
	public string Type { get; set; } = "";
	[JsonPropertyName("nullable")]
	public bool Nullable { get; set; }
	[JsonPropertyName("isPK")]
	public bool IsPK { get; set; }
	[JsonPropertyName("isFK")]
	public bool IsFK { get; set; }
	[JsonPropertyName("isUK")]
	public bool IsUK { get; set; }
	[JsonPropertyName("isAutoIncrement")]
	public bool IsAutoIncrement { get; set; }
	[JsonPropertyName("defaultValue")]
	public string DefaultValue { get; set; } = "";
	[JsonPropertyName("fkReferencesTable")]
	public string? FkReferencesTable { get; set; }
	[JsonPropertyName("fkReferencesColumn")]
	public string? FkReferencesColumn { get; set; }
}

public class Relationship
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = "";
	[JsonPropertyName("sourceTable")]
	public string SourceTable { get; set; } = "";
	[JsonPropertyName("sourceColumn")]
	public string SourceColumn { get; set; } = "";
	[JsonPropertyName("targetTable")]
	public string TargetTable { get; set; } = "";
	[JsonPropertyName("targetColumn")]
	public string TargetColumn { get; set; } = "";
	[JsonPropertyName("cardinality")]
	public string Cardinality { get; set; } = "";
}
