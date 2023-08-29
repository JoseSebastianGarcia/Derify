using Derify.Core.Repository;
using Derify.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derify.Core
{
    public static class DerifyExtensions
    {
        public static void AddDerify(this IServiceCollection services, string connectionString) 
        {
            services.AddScoped<IBaseRepository>(serviceProvider => new BaseRepository(connectionString));
            services.AddScoped<IDerifyService,DerifyService>();
        }

        public static void UseDerify(this IApplicationBuilder app) 
        {
            app.Map("/Derify", app => {
                app.UseMiddleware<DerifyUIMiddleware>();
            });
        }
    }
}
