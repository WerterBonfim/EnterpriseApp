using System;
using System.Net.Mail;

namespace NSE.Core.DomainObjects
{
    public class Email
    {
        public const int EnderecoMaxLenght = 254;
        public const int EnderecoMinLenght = 5;

        // EF Core
        protected Email()
        {
        }

        public Email(string endereco)
        {
            if (!Validar(endereco))
                throw new DomainException("E-mail inválido");

            Endereco = endereco;
        }

        public string Endereco { get; }

        public static bool Validar(string endereco)
        {
            try
            {
                new MailAddress(endereco);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}