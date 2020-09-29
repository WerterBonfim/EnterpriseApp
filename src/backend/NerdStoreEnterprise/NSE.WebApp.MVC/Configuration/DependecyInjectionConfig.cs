using System.Net.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependecyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>()
                .ConfigureHttpMessageHandlerBuilder(x =>
                {
                    x.PrimaryHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                });

            

            //services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();


            services.AddHttpClient<ICatalogoService, CatalogoService2>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .ConfigureHttpMessageHandlerBuilder(x =>
                {
                    x.PrimaryHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                });
        }
    }
}