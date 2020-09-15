using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using NSE.Identidade.API.Data;

namespace NSE.Identidade.API.Configuration
{
    public class PrepararDb
    {
        public static void RodarMigrationInicial(IApplicationBuilder app)
        {
            using (var scopo = app.ApplicationServices.CreateScope())
            {
                RodarMigrations(scopo.ServiceProvider.GetService<ApplicationDbContext>());
            }
        }

        private static void RodarMigrations(ApplicationDbContext context)
        {
            Console.WriteLine("Verificando se o banco já existe");
            if (context.Database.GetService<IRelationalDatabaseCreator>().Exists())
            {
                Console.WriteLine("Banco já existe");
                return;
            }

            Console.WriteLine("Iniciando o migrations");
            if (context.Database.GetMigrations().Any())
                context.Database.Migrate();
        }
    }
}