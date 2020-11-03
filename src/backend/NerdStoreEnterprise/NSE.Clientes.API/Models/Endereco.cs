using System;
using NSE.Core.DomainObjects;

namespace NSE.Clientes.API.Models
{
    public class Endereco : Entity, IAggregateRoot
    {
        protected Endereco()
        {
        }

        public Endereco(string logradouro, string numero, string complemento, string bairro, string cep, string estado,
            Guid clienteId)
        {
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cep = cep;
            Estado = estado;
            ClienteId = clienteId;
        }

        public string Logradouro { get; }
        public string Numero { get; }
        public string Complemento { get; }
        public string Bairro { get; }
        public string Cep { get; }
        public string Estado { get; }
        

        // EF Relacionamento
        public Guid ClienteId { get; }
        public Cliente Cliente { get; protected set; }
    }
}