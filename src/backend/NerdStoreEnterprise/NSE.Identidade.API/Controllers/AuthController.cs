using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSE.Identidade.API.Extensions;
using NSE.Identidade.API.Models;

namespace NSE.Identidade.API.Controllers
{
    [Route("api/identidade")]
    public class AuthController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }


        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return RespostaPersonalizada(ModelState);

            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                var login = await GerarJwt(usuarioRegistro.Email);
                return Ok(RespostaPersonalizada(login));
            }

            foreach (var erro in result.Errors)
                AdicionarErro(erro.Description);

            return RespostaPersonalizada();
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin)
        {
            
            if (!ModelState.IsValid) return RespostaPersonalizada(ModelState);

            var result = await _signInManager.PasswordSignInAsync(
                usuarioLogin.Email,
                usuarioLogin.Senha,
                false,
                true
            );


            if (result.Succeeded)
            {
                var login = await GerarJwt(usuarioLogin.Email);
                return RespostaPersonalizada(login);
            }


            
            if (result.IsLockedOut)
                AdicionarErro("Usuário temporariamente bloqueado por tentativas inválidas");

            AdicionarErro("Usuário ou senha incorretos");

            return RespostaPersonalizada();
        }


        private async Task<UsuarioRepostaLogin> GerarJwt(string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(usuario);
            var roles = await _userManager.GetRolesAsync(usuario);
            

            ConfigurarClaims(claims, usuario, roles);
            var compactToken = GerarCompactToken(claims);
            var resposta = MontarRespostaLogin(compactToken, usuario, claims);

            return resposta;
        }

        private UsuarioRepostaLogin MontarRespostaLogin(string compactToken, IdentityUser usuario, IList<Claim> claims)
        {
            var resposta = new UsuarioRepostaLogin
            {
                AccessToken = compactToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    Claims = claims
                        .Select(x => new UsuarioClaim
                        {
                            Type = x.Type,
                            Value = x.Value
                        })
                }
            };
            return resposta;
        }

        private string GerarCompactToken(IList<Claim> claims)
        {
            var identityClaim = new ClaimsIdentity();
            identityClaim.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaim,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var compactToken = tokenHandler.WriteToken(token);
            return compactToken;
        }

        private static void ConfigurarClaims(IList<Claim> claims, IdentityUser usuario, IList<string> roles)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, usuario.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, usuario.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf,
                ToUnixEpochDate(DateTime.UtcNow).ToString()));

            claims.Add(new Claim(JwtRegisteredClaimNames.Iat,
                ToUnixEpochDate(DateTime.UtcNow).ToString(),
                ClaimValueTypes.Integer64));

            foreach (var role in roles)
                claims.Add(new Claim("role", role));
        }

        private static DateTimeOffset UnixEpoch =
            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        private static long ToUnixEpochDate(DateTime data)
            => (long) Math.Round((data.ToUniversalTime() - UnixEpoch).TotalSeconds);
    }
}