using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NSE.WebApp.MVC.Extensions;

namespace NSE.WebApp.MVC.Services
{
    public abstract class Service
    {
        protected StringContent SerializarConteudo(object obj)
        {
            return new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8,
                "application/json"
            );
        }

        protected async Task<T> Deserializar<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var conteudo = await responseMessage.Content.ReadAsStringAsync();
            var objeto = JsonSerializer.Deserialize<T>(conteudo, options);
            return objeto;
        }

        // Antigo nome: TratarErrosResponse
        protected bool TrataEVerificaSeOuveErros(HttpResponseMessage response)
        {
            switch ((int) response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    return true;
            }

            response.EnsureSuccessStatusCode();
            return false;
        }
    }
}