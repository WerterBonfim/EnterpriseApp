using System;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Clientes.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.Core.Messages.Integrations;

namespace NSE.Clientes.API.Services
{
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IBus _bus;

        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Iniciando o serviço hospeado com RabbitMQ");

            try
            {
                _bus = RabbitHutch.CreateBus("host=localhost:5672");

                _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(
                    async request =>
                        new ResponseMessage(await RegistrarCliente(request)));
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocorreu um erro ao tentar iniciar o serviço hospedado com RabbitMQ." +
                                  "Erro: " + e.Message);

                throw e;
            }

            return Task.CompletedTask;
        }

        private async Task<ValidationResult> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);

            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
            var result = await mediator.EnviarComando(clienteCommand);

            return result;
        }
    }
}