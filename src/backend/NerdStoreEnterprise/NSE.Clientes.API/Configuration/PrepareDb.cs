using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using NSE.Clientes.API.Data;

namespace NSE.Clientes.API.Configuration
{
    /// <summary>
    ///     3º Na fila do migration.
    ///     Deve ser a terceira a API a subir na ordem correta.
    /// </summary>
    public class PrepararDb
    {
        private static readonly string _nomeApp = "Clientes.API";

        public static void RodarMigrationInicial(IApplicationBuilder app)
        {
            using (var scopo = app.ApplicationServices.CreateScope())
            {
                RodarMigrations(scopo.ServiceProvider.GetService<ClientesContext>());
            }
        }

        private static void RodarMigrations(ClientesContext context)
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