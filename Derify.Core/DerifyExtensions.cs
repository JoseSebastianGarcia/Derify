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
	private static PathString _apiBasePath = "/api/derify/ss-model";
	public static void AddDerify(this IServiceCollection services, string connectionString) 
    {
        services.AddScoped<IBaseRepository>(serviceProvider => new BaseRepository(connectionString));
        services.AddScoped<IDerifyService,DerifyService>();
    }


	public static IApplicationBuilder UseDerify(this IApplicationBuilder app) 
    {
		// Provider embebido (namespace donde están los recursos) 
		var embeddedProvider = new EmbeddedFileProvider(DerifyCoreAssembly.Assembly, "Derify.Core.wwwroot");

		// Provider físico: carpeta wwwroot junto a la ubicación del assembly (útil en desarrollo/publish donde se copian archivos)
		var assemblyDir = Path.GetDirectoryName(DerifyCoreAssembly.Assembly.Location) ?? AppContext.BaseDirectory;
		var physicalWwwRoot = Path.Combine(assemblyDir, "wwwroot");
		IFileProvider physicalProvider = Directory.Exists(physicalWwwRoot) ? new PhysicalFileProvider(physicalWwwRoot) : new NullFileProvider();

		// Composite para cubrir ambos casos (si no existe, instala Microsoft.Extensions.FileProviders.Composite o usa FallbackFileProvider)
		var fileProvider = new CompositeFileProvider(physicalProvider, embeddedProvider);

		// UI (SPA) mapeada en /derify (incluye subrutas)
		app.Map(_uiBasePath, branch =>
		{
			// Servir directamente /index.html (antes se redirigía; eso puede provocar bucles)
			branch.Map("/index.html", b =>
			{
				b.Run(async context =>
				{
					const string indexFileName = "index.html";
					var indexFile = fileProvider.GetFileInfo(indexFileName);
					if (!indexFile.Exists)
					{
						context.Response.StatusCode = StatusCodes.Status404NotFound;
						await context.Response.WriteAsync($"'{indexFileName}' not found.");
						return;
					}

					context.Response.ContentType = "text/html; charset=utf-8";
					context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
					context.Response.ContentLength = indexFile.Length;

					await using var stream = indexFile.CreateReadStream();
					await stream.CopyToAsync(context.Response.Body);
				});
			});

			// Servir assets estáticos (js, css, images, derify.config.json, etc.)
			branch.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = fileProvider
			});

			// Si piden exactamente "/derify" (sin slash final), redirigir a "/derify/" para que las rutas relativas en index.html funcionen
			branch.Run(async context =>
			{
				// Cuando Map se aplica, el Path dentro del branch es el "resto" después del prefijo.
				var remainder = context.Request.Path;
				if (!remainder.HasValue || remainder == PathString.Empty)
				{
					var basePath = context.Request.PathBase.HasValue ? context.Request.PathBase.Value : string.Empty;
					context.Response.Redirect(basePath + "/", false);
					return;
				}

				// Endpoint opcional para devolver configuración si existe como recurso embebido/físico:
				if (remainder.Value!.Equals("/derify.config.json", StringComparison.OrdinalIgnoreCase))
				{
					var cfg = fileProvider.GetFileInfo("derify.config.json");
					if (!cfg.Exists)
					{
						context.Response.StatusCode = StatusCodes.Status404NotFound;
						return;
					}

					context.Response.ContentType = "application/json; charset=utf-8";
					context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
					context.Response.ContentLength = cfg.Length;
					await using var cs = cfg.CreateReadStream();
					await cs.CopyToAsync(context.Response.Body);
					return;
				}

				// Fallback: devolver index.html para que React Router maneje la ruta cliente
				const string indexFileName = "index.html";
				var index = fileProvider.GetFileInfo(indexFileName);
				if (!index.Exists)
				{
					context.Response.StatusCode = StatusCodes.Status404NotFound;
					await context.Response.WriteAsync($"'{indexFileName}' not found.");
					return;
				}

				context.Response.ContentType = "text/html; charset=utf-8";
				context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
				context.Response.ContentLength = index.Length;

				await using var stream = index.CreateReadStream();
				await stream.CopyToAsync(context.Response.Body);
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
			var databaseSchemaResult = service.GetDatabaseSchema();

			var json = JsonSerializer.Serialize(databaseSchemaResult);
			await SendResponse(context, databaseSchemaResult.IsSuccess ? 200 : 500, json);
		});
        
        return routeBuilder;
    }

	private async static Task SendResponse(HttpContext context, int httpStatusCode, string body) 
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = httpStatusCode;
		context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
		context.Response.ContentLength = body.Length;
		await context.Response.WriteAsync(body);
	}
}