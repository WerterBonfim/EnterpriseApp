using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using NSE.Catalogo.API.Data;

namespace NSE.Catalogo.API.Configuration
{
    public class PrepararDb
    {
        private static string _nomeApi = "Catálogo.API";
        
        public static void RodarMigrationInicial(IApplicationBuilder app)
        {
            using (var scopo = app.ApplicationServices.CreateScope())
            {
                RodarMigrations(scopo.ServiceProvider.GetService<CatalogoContext>());
            }
        }

        private static void RodarMigrations(CatalogoContext context)
        {
            Console.WriteLine($"{_nomeApi}: Verificando se o banco já existe");
            if (context.Database.GetService<IRelationalDatabaseCreator>().Exists())
            {
                Console.WriteLine("Banco já existe");
                return;
            }

            Console.WriteLine($"{_nomeApi}: Iniciando o migrations");
            if (context.Database.GetMigrations().Any())
                context.Database.Migrate();
        }
    }
}