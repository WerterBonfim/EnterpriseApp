using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace NSE.WebApp.MVC.Extensions
{
    public interface IUser
    {
        string Nome { get; }
        Guid ObterId();
        string ObterEmail();
        string ObterToken();
        bool EstaAutenticado();
        bool PossuiRole(string role);
        IEnumerable<Claim> ListarClaims();
        HttpContext ObterContext();
    }

    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor httpContextAccessor)
        {
            _accessor = httpContextAccessor;
        }

        public string Nome => _accessor.HttpContext.User.Identity.Name;

        public Guid ObterId()
        {
            return !EstaAutenticado() ? Guid.Empty : Guid.Parse(_accessor.HttpContext.User.GetUserId());
        }

        public string ObterEmail()
        {
            return !EstaAutenticado() ? "" : _accessor.HttpContext.User.GetUserEmail();
        }

        public string ObterToken()
        {
            return !EstaAutenticado() ? "" : _accessor.HttpContext.User.GetUserToken();
        }

        public bool EstaAutenticado()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool PossuiRole(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }

        public IEnumerable<Claim> ListarClaims()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public HttpContext ObterContext()
        {
            return _accessor.HttpContext;
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return GetClaimByName(principal, "sub");
        }


        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            return GetClaimByName(principal, "email");
        }

        public static string GetUserToken(this ClaimsPrincipal principal)
        {
            return GetClaimByName(principal, "JWT");
        }


        private static string GetClaimByName(ClaimsPrincipal principal, string name)
        {
            if (principal is null)
                throw new ArgumentException(nameof(principal));

            var claim = principal.FindFirst(name);
            return claim?.Value;
        }
    }
}