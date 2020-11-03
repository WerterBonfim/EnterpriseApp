using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Controllers
{
    public abstract class BaseController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            var temErros = resposta != null && resposta.Errors.Mensagens.Any();
            if (!temErros)
                return false;

            foreach (var mensagem in resposta.Errors.Mensagens)
                ModelState.AddModelError(string.Empty, mensagem);

            return true;
        }
    }
}