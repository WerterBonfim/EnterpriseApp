using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.WebApi.Core.Identidade;

namespace NSE.Identidade.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services.AddControllers();
            
            return services;
        }
        
        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Configurações do Identity deve iniciar aqui, entre as rotas e o endpoint.
            // é uma exigência do asp.net core
            app.UseAuthConfiguration(env);

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            PrepararDb.RodarMigrationInicial(app);

            return app;
        }
    }
}