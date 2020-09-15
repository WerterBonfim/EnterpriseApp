using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NSE.Identidade.API.Controllers
{
    [ApiController]
    public abstract class BaseController : Controller
    {
        private ICollection<string> _erros = new List<string>();

        protected IActionResult RespostaPersonalizada(object resultado = null)
        {
            if (OperacaoValida())
                return Ok(resultado);

            var erros = new Dictionary<string, string[]>
            {
                {"Mensagens", _erros.ToArray()}
            };
            
            return BadRequest(new ValidationProblemDetails(erros));
        }

        protected IActionResult RespostaPersonalizada(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(x => x.Errors);
            foreach (var erro in erros)
                AdicionarErro(erro.ErrorMessage);
            
            return RespostaPersonalizada();
        }

        protected bool OperacaoValida()
        {
            return !_erros.Any();
        }

        protected void AdicionarErro(string erro) => _erros.Add(erro);
        protected void LimparErros() => _erros.Clear();
    }
}