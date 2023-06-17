using System.Net;

namespace Noticias.Services
{
    public class ExceptionService : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public int StatusCodeInt { get => (int)this.StatusCode; }

        public ExceptionService(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
