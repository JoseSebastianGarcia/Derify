using Derify.Core.Reponse;

namespace Derify.Core.Services;

public interface IDerifyService
{
	GetDatabaseSchemaResponse GetDatabaseSchema();
}
