using System.Text.RegularExpressions;

namespace NSE.Core.DomainObjects
{
    public class Email
    {
        private readonly Regex emailRegex = new Regex("(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+" +
                                                      "(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]" +
                                                      "|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a" +
                                                      "-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|" +
                                                      "[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\" +
                                                      "[\x01-\x09\x0b\x0c\x0e-\x7f])+)\\])");


        public const int EnderecoMaxLenght = 254;
        public const int EnderecoMinLenght = 5;

        public string Endereco { get; private set; }

        protected Email()
        {
        }

        public Email(string endereco)
        {
            if (!Validar(endereco)) 
                throw new DomainException("E-mail inválido");

            Endereco = endereco;
        }

        private bool Validar(string endereco)
        {
            return emailRegex.IsMatch(endereco);
        }
    }
}