using Derify.Core.Repository;
using Derify.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Unicode;

namespace Derify.Core
{
    public static class DerifyExtensions
    {
        public static void AddDerify(this IServiceCollection services, string connectionString) 
        {
            services.AddScoped<IBaseRepository>(serviceProvider => new BaseRepository(connectionString));
            services.AddScoped<IDerifyService,DerifyService>();
        }

        public static void UseDerify(this IApplicationBuilder app,Action<DerifyOptions> optionsHandler) 
        {
            DerifyOptions options = new DerifyOptions();
            
            optionsHandler(options);
            
            app.Map($"{options.PathMatch}", app => {
                app.UseMiddleware<DerifyUIMiddleware>(options);
            });
        }
        public static void UseDerify(this IApplicationBuilder app)
        {
            app.UseDerify(opts => { });
        }
    }
}
