using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public class CatalogoService2 : Service, ICatalogoService
    {
        private readonly IFlurlRequest _catalogoApi;

        public CatalogoService2(
            IFlurlClientFactory flurlClientFactory,
            IOptions<AppSettings> settings
            )
        {
            _catalogoApi = flurlClientFactory
                .Get(settings.Value.CatalogoUrl)
                .Request("catalogo");

        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            var response = await _catalogoApi
                .AppendPathSegment("produto")
                .GetJsonAsync<IEnumerable<ProdutoViewModel>>();

            return response;
        }

        public async Task<ProdutoViewModel> ObterProId(Guid id)
        {
            var response = await _catalogoApi
                .AppendPathSegment("produto")
                .AppendPathSegment(id.ToString())
                .GetJsonAsync<ProdutoViewModel>();

            return response;
        }
    }
}