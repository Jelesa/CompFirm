using System;
using System.Net;

namespace CompFirm.Domain.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }

        public ApiException(string message, HttpStatusCode httpStatusCode)
            : base(message)
        {
            this.HttpStatusCode = httpStatusCode;
        }
    }
}
