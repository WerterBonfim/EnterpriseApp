using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Clientes.API.Models;

namespace NSE.Clientes.API.Data
{
    public sealed class ClientesContext : DbContext, IUnitOfWork
    {
        public ClientesContext(DbContextOptions<ClientesContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var todasAsPropriedades = modelBuilder.Model.GetEntityTypes()
                .SelectMany(x =>
                    x.GetProperties().Where(p => p.ClrType == typeof(string)));
            
            foreach (var property in todasAsPropriedades)
                property.SetColumnType("varchar(100)");


            var relacionamentos = modelBuilder.Model.GetEntityTypes()
                .SelectMany(x => x.GetForeignKeys());

            foreach (var relacionamento in relacionamentos)
                relacionamento.DeleteBehavior = DeleteBehavior.ClientSetNull;


            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientesContext).Assembly);
        }
        
        public async Task<bool> Commit()
        {
            return await SaveChangesAsync() > 0;
        }
    }
}