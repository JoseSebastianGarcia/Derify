using Microsoft.AspNetCore.Http;

namespace Derify.Core;

public class DerifyOptions
{
    public PathString UiPathMatch { get; init; } = "/derify";
	public PathString ApiPathMatch { get; init; } = "/api/derify";

	private DerifyOptions(string uiPathMatch, string apiPathMatch) 
		=> (UiPathMatch, ApiPathMatch) = (uiPathMatch, apiPathMatch);
	public static DerifyOptions Create(string uiPathMatch, string apiPathMatch) => new DerifyOptions(uiPathMatch, apiPathMatch);
}

