using System;
using FluentValidation;
using NSE.Core.DomainObjects;
using NSE.Core.Messages;

namespace NSE.Clientes.API.Application.Commands
{
    public class RegistrarClienteCommand : Command
    {
        public RegistrarClienteCommand(Guid id, string nome, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Nome = nome;
            Email = email;
            Cpf = cpf;
        }

        public Guid Id { get; }
        public string Nome { get; }
        public string Email { get; }
        public string Cpf { get; }

        public override bool EValido()
        {
            ValidationResult = new ValidacaoParaRegistrarUmCliente().Validate(this);
            return ValidationResult.IsValid;
        }


        private sealed class ValidacaoParaRegistrarUmCliente : AbstractValidator<RegistrarClienteCommand>
        {
            public ValidacaoParaRegistrarUmCliente()
            {
                RuleFor(x => x.Id)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente inválido");

                RuleFor(x => x.Nome)
                    .NotEmpty()
                    .WithMessage("O nome do cliente não foi informado.");

                RuleFor(x => x.Cpf)
                    .Must(TerCpfValido)
                    .WithMessage("O CPF informado não é válido.");

                RuleFor(x => x.Email)
                    .Must(TerEmailValido)
                    .WithMessage("O e-mail informado não é valido.");
            }


            private static bool TerEmailValido(string email)
            {
                return Core.DomainObjects.Email.Validar(email);
            }

            private static bool TerCpfValido(string cpf)
            {
                return CPF.EValido(cpf);
            }
        }
    }
}