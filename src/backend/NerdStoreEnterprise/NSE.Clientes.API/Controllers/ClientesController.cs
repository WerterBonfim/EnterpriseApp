using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSE.Clientes.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.WebApi.Core.Controllers;

namespace NSE.Clientes.API.Controllers
{
    public class ClientesController : BaseController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public ClientesController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("clientes")]
        public async Task<IActionResult> Index()
        {
            var tarefa = await _mediatorHandler.EnviarComando(
                new RegistrarClienteCommand(
                    Guid.NewGuid(),
                    "werter",
                    "werter@hotmail.com.br",
                    "36726916825"));

            return RespostaPersonalizada(tarefa);
        }
    }
}