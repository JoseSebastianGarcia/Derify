using Derify.Core.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Derify.Core
{
    public class DerifyUIMiddleware
    {
        private readonly DerifyOptions _options;

        public DerifyUIMiddleware(RequestDelegate next, DerifyOptions options)
        {
            _options = options;
        }

        public void GetStream(HttpContext context, string fileName, string code = "") 
        {
            string resourcePrefix = "Derify.Core.wwwroot";

            Assembly assembly = typeof(DerifyUIMiddleware).GetTypeInfo().Assembly;

            string resourceToSend = string.Empty;

            string[] resourceItems = assembly.GetManifestResourceNames();
            foreach (string resourceItem in resourceItems) 
            {
                string requestedResource = $"{resourcePrefix}.{fileName.Replace('/','.')}".Trim().ToUpper();
                string existingResource = resourceItem.Trim().ToUpper();
                
                if(requestedResource == existingResource) 
                {
                    resourceToSend = resourceItem;
                    break;
                }
            }
            
            if (resourceToSend == string.Empty) throw new FileNotFoundException("No se encontró el recurso solicitado.",fileName);

            Stream? stream = assembly.GetManifestResourceStream(resourceToSend);

            if (stream != null)
            {
                using (StreamReader reader = new StreamReader(stream))
                {

                    // Inject arguments before writing to response
                    var htmlBuilder = new StringBuilder(reader.ReadToEnd());

                    if (htmlBuilder != null)
                    {
                        if(code != string.Empty)
                            htmlBuilder.Replace("$(mermaid.content)", code);

                        htmlBuilder.Replace("$(mermaid.path)", _options.PathMatch.Value.Substring(1, _options.PathMatch.Value.Length - 1));

                        context.Response.StatusCode = 200;
                        context.Response.ContentType = GetContentType(fileName);
                        context.Response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
                        
                    }
                }
            }
        }
        private string GetContentType(string fileName)
        {
            // Mapea la extensión del archivo a su tipo de contenido correspondiente
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".html":
                    return "text/html; charset=utf-8";
                case ".js":
                    return "text/javascript";
                case ".json":
                    return "application/json";
                case ".css":
                    return "text/css";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".ico":
                    return "image/vnd.microsoft.icon";
                case ".ttf":
                    return "font/ttf";
                default:
                    throw new FormatException("Formato de archivo inválido");
            }
        }
        public Task Invoke(HttpContext httpContext, IDerifyService service)
        {
            var request = httpContext.Request;

            if(!request.Path.HasValue)
                GetStream(httpContext,"index.html",service.GetCode());
            else
                GetStream(httpContext, request.Path.Value.TrimStart('/'));


            return Task.CompletedTask;
        }

        
    }
}
