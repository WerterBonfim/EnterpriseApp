using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;
         

        public AutenticacaoService(
            HttpClient httpClient,
            IOptions<AppSettings> settings
            )
        {
            httpClient.BaseAddress = new Uri(settings.Value.AutenticacaoUrl);
            _httpClient = httpClient;
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = SerializarConteudo(usuarioLogin);
            var endpoint = "/api/identidade/autenticar";
            var resposta = await _httpClient.PostAsync(endpoint, loginContent);
            return await TratarRequest(resposta);
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {
            var loginContent = SerializarConteudo(usuarioRegistro);

            var endpoint = "/api/identidade/nova-conta";
            var resposta = await _httpClient.PostAsync(endpoint, loginContent);
            var response = await TratarRequest(resposta);
            return response;
        }

        private async Task<UsuarioRespostaLogin> TratarRequest(HttpResponseMessage responseMessage)
        {
            var temErros = TrataEVerificaSeOuveErros(responseMessage);
            var respostaLogin = temErros
                ? new UsuarioRespostaLogin
                {
                    ResponseResult = await Deserializar<ResponseResult>(responseMessage)
                }
                : await Deserializar<UsuarioRespostaLogin>(responseMessage);


            return respostaLogin;
        }
    }
}