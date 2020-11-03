using System;

namespace NSE.Core.DomainObjects
{
    public class CPF
    {
        public const int QtdMaximaDeCaracteres = 11;
        public string Numero { get; }

        // EF Core
        public CPF()
        {
        }

        public CPF(string numeros)
        {
            numeros = VerificaOCpf(numeros);
            Numero = numeros;
        }

        

        private static string VerificaOCpf(string numeros)
        {
            var cpfValido = EValido(numeros);

            if (!cpfValido)
                throw new DomainException("CPF inválido");

            numeros = RemoverPontos(numeros);

            return numeros;
        }

        /// <summary>
        ///     Verifica se o Cpf informado é valido.
        /// </summary>
        /// <param name="numeros"></param>
        /// <returns>Se é ou não é valido</returns>
        public static bool EValido(string numeros)
        {
            if (string.IsNullOrEmpty(numeros) || string.IsNullOrWhiteSpace(numeros))
                return false;

            numeros = RemoverPontos(numeros);

            var cpfInvalido = numeros.Length > 11 || numeros.Length < 11;
            if (cpfInvalido)
                return false;

            cpfInvalido = !ValidarCPF(numeros);
            return !cpfInvalido;
        }

        private static string RemoverPontos(string cpf)
        {
            return cpf.Replace(".", "")
                .Replace("-", "")
                .Trim();
        }

        // https://www.eximiaco.ms/pt/2020/01/10/no-c-8-ficou-mais-facil-alocar-arrays-na-stack-e-isso-pode-ter-um-impacto-positivo-tremendo-na-performance/
        private static bool ValidarCPF(string sourceCPF)
        {
            static bool VerificaTodosValoresSaoIguais(ref Span<int> input)
            {
                for (var i = 1; i < 11; i++)
                    if (input[i] != input[0])
                        return false;

                return true;
            }

            if (string.IsNullOrWhiteSpace(sourceCPF))
                return false;

            Span<int> cpfArray = stackalloc int[11];
            var count = 0;

            foreach (var c in sourceCPF)
                if (char.IsDigit(c))
                {
                    if (count > 10) return false;

                    cpfArray[count] = c - '0';
                    count++;
                }

            if (count != 11) return false;
            if (VerificaTodosValoresSaoIguais(ref cpfArray)) return false;


            var totalDigitoI = 0;
            var totalDigitoII = 0;
            int modI;
            int modII;

            for (var posicao = 0; posicao < cpfArray.Length - 2; posicao++)
            {
                totalDigitoI += cpfArray[posicao] * (10 - posicao);
                totalDigitoII += cpfArray[posicao] * (11 - posicao);
            }

            modI = totalDigitoI % 11;
            if (modI < 2)
                modI = 0;
            else
                modI = 11 - modI;

            if (cpfArray[9] != modI) return false;

            totalDigitoII += modI * 2;

            modII = totalDigitoII % 11;
            if (modII < 2)
                modII = 0;
            else
                modII = 11 - modII;

            return cpfArray[10] == modII;
        }
    }
}