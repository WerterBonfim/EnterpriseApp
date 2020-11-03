using System;

namespace NSE.Core.Messages.Integrations
{
    public class UsuarioRegistradoIntegrationEvent : IntegrationEvent
    {
        public UsuarioRegistradoIntegrationEvent(Guid id, string nome, string email, string cpf)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Cpf = cpf;
        }

        public Guid Id { get; }
        public string Nome { get; }
        public string Email { get; }
        public string Cpf { get; }
    }
}