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
        private static readonly string _nomeApp = "Identidade.API";

        public static void RodarMigrationInicial(IApplicationBuilder app)
        {
            using (var scopo = app.ApplicationServices.CreateScope())
            {
                RodarMigrations(scopo.ServiceProvider.GetService<ApplicationDbContext>());
            }
        }

        private static void RodarMigrations(ApplicationDbContext context)
        {
            Informar("Verificando se a migrations pendentes...");
            var bancoNaoExiste = !context.Database.GetService<IRelationalDatabaseCreator>().Exists();
            var temMigrationsPendendente = context.Database.GetPendingMigrations().Any();
            var temAlgoPendente = bancoNaoExiste || temMigrationsPendendente;

            if (!temAlgoPendente) return;

            Informar("Rodando as migrations");
            context.Database.Migrate();
        }

        private static void Informar(string texto)
        {
            Console.WriteLine($"{_nomeApp}: {texto}");
        }
    }
}