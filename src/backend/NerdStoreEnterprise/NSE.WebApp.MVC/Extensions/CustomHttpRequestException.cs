using System;
using System.Net;

namespace NSE.WebApp.MVC.Extensions
{
    public class CustomHttpRequestException : Exception
    {
        public CustomHttpRequestException()
        {
        }

        public CustomHttpRequestException(string mensagem, Exception innerException)
            : base(mensagem, innerException)
        {
        }

        public CustomHttpRequestException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}