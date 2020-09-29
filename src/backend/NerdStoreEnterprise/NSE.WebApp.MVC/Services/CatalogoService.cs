using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public class CatalogoService : Service, ICatalogoService
    {
        private readonly HttpClient _httpClient;

        public CatalogoService(
            HttpClient httpClient,
            IOptions<AppSettings> settings
        )
        {
            httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
            _httpClient = httpClient;
        }
        
        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            var response = await _httpClient.GetAsync("/catalogo/produtos");
            TrataEVerificaSeOuveErros(response);
            return await Deserializar<IEnumerable<ProdutoViewModel>>(response);
        }

        public async Task<ProdutoViewModel> ObterProId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");
            TrataEVerificaSeOuveErros(response);
            return await Deserializar<ProdutoViewModel>(response);
        }
    }
}