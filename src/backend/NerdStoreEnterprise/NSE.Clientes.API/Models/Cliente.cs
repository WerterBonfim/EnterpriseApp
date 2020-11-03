using System;
using NSE.Core.DomainObjects;

namespace NSE.Clientes.API.Models
{
    public class Cliente : Entity, IAggregateRoot
    {
        protected Cliente()
        {
        }

        public Cliente(Guid id, string nome, string email, string cpf)
        {
            Id = id;
            Nome = nome;
            Email = new Email(email);
            Cpf = new CPF(cpf);
        }

        public string Nome { get; }
        public Email Email { get; private set; }
        public CPF Cpf { get; private set; }
        public bool Excluido { get; private set; }
        public Guid EnderecoId { get; private set; }
        public Endereco Endereco { get; private set; }

        public void TrocarEmail(string email)
        {
            Email = new Email(email);
        }

        public void AtribuirEndereco(Endereco endereco)
        {
            Endereco = endereco;
        }
    }
}