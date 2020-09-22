using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace NSE.EFHelpers
{
    public class MigrationHelper
    {
        public static void RodarMigrationInicial(IApplicationBuilder app, Type obj)
        {
            var teste = app.ApplicationServices.GetService(obj);

            using (var scopo = app.ApplicationServices.CreateScope())
            {
                RodarMigrations(scopo.ServiceProvider.GetService(typeof(DbContext)) as DbContext );
            }
        }

        private static void RodarMigrations(DbContext context)
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