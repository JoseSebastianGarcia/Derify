using Derify.Core.Repository;
using Derify.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Text.Json;


namespace Derify.Core;

public static class DerifyExtensions
{
    private static PathString _uiBasePath = "/derify";
	private static PathString _apiBasePath = "/api/derify";
	public static void AddDerify(this IServiceCollection services, string connectionString) 
    {
        services.AddScoped<IBaseRepository>(serviceProvider => new BaseRepository(connectionString));
        services.AddScoped<IDerifyService,DerifyService>();
    }
    public static IApplicationBuilder UseDerify(this IApplicationBuilder app) 
    {
        return app.UseDerify(_ => { }); //Imple por defecto si el usuario no quiere pasar opciones
    }

	public static IApplicationBuilder UseDerify(this IApplicationBuilder app, Action<DerifyOptions> optionsHandler) 
    {
			app.Map(_uiBasePath, branch =>
			{
				branch.UseStaticFiles(new StaticFileOptions
				{
					FileProvider = new EmbeddedFileProvider(
						DerifyCoreAssembly.Assembly, "Derify.Core.wwwroot")
				});

				branch.Run(async context =>
				{
					await context.Response.SendFileAsync("index.html");
				});
			});

			app.Map(_apiBasePath, branch =>
			{
				branch.UseRouter(routeBuilder =>
				{
					routeBuilder.UseDerifyApi();
				});
			});

		return app;
    }
    public static IRouteBuilder UseDerifyApi(this IRouteBuilder routeBuilder) 
    {
		routeBuilder.MapGet("", async (HttpContext context) =>
		{
			IDerifyService service = context.RequestServices.GetRequiredService<IDerifyService>();
			var databaseSchema = service.GetDatabaseSchema();
			var json = JsonSerializer.Serialize(databaseSchema);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = 200;
			context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
			context.Response.ContentLength = json.Length;

			await context.Response.WriteAsync(json);
			await Task.CompletedTask;
		});
        
        return routeBuilder;
    }
}