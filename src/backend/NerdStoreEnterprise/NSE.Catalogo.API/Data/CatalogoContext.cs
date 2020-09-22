using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Models;
using NSE.Core.Data;

namespace NSE.Catalogo.API.Data
{
    public class CatalogoContext : DbContext, IUnitOfWork
    {
        public CatalogoContext(DbContextOptions<CatalogoContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var todasAsPropriedades = modelBuilder.Model.GetEntityTypes()
                .SelectMany(x =>
                    x.GetProperties().Where(p => p.ClrType == typeof(string)));
            
            foreach (var property in todasAsPropriedades)
                property.SetColumnType("varchar(100)");
            
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            return await SaveChangesAsync() > 0;
        }
    }
}