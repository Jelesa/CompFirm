using System.Net;

namespace CompFirm.Domain.Exceptions
{
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException()
            : base("Произошла ошибка авторизации", HttpStatusCode.Unauthorized)
        {
        }

        public UnauthorizedException(string message)
            : base(message, HttpStatusCode.Unauthorized)
        {
        }
    }
}
