using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NSE.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (CustomHttpRequestException exception)
            {
                HandlerRequestExceptionAsync(httpContext, exception);
            }
        }

        private static void HandlerRequestExceptionAsync(HttpContext context, CustomHttpRequestException httpRequestException)
        {
            if (httpRequestException.StatusCode == HttpStatusCode.Unauthorized)
            {
                context.Response.Redirect($"/login?returnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int)httpRequestException.StatusCode;
        }
    }
}