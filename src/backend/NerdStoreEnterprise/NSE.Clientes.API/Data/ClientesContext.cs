using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Models;
using NSE.Core.Data;
using NSE.Core.DomainObjects;
using NSE.Core.Mediator;
using NSE.Core.Messages;

namespace NSE.Clientes.API.Data
{
    public sealed class ClientesContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public ClientesContext(DbContextOptions<ClientesContext> options, IMediatorHandler mediatorHandler) :
            base(options)
        {
            _mediatorHandler = mediatorHandler;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        public async Task<bool> Commit()
        {
            var sucesso = await SaveChangesAsync() > 0;
            if (!sucesso) return false;

            await _mediatorHandler.PublicarEventos(this);

            return true;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

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
    }

    public static class MediatorExtension
    {
        public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T contexto) where T : DbContext
        {
            var domainEntities = contexto.ChangeTracker
                .Entries<Entity>()
                .Where(x =>
                    x.Entity.Notificações != null &&
                    x.Entity.Notificações.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notificações)
                .ToList();

            domainEntities.ToList()
                .ForEach(x => x.Entity.LimparEventos());

            // var tasks = domainEvents
            //     .Select(async (domainEvent) => await mediator.PublicarEvento(domainEvent));

            async Task PublicarEvento(Event domainEvent)
            {
                await mediator.PublicarEvento(domainEvent);
            }

            var tasks = domainEvents
                .Select(PublicarEvento);

            await Task.WhenAll(tasks);
        }
    }
}