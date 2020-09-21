using System.Threading.Tasks;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        public Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin);
        public Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro);
    }
}