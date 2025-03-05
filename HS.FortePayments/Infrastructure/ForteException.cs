using System;
using System.Net;

namespace Forte
{
    [Serializable]
    public class ForteException : ApplicationException
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public ForteError ForteError { get; set; }

        public ForteException()
        {
            ForteError = new ForteError();
        }

        public ForteException(HttpStatusCode httpStatusCode, ForteError forteError, string message)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
            ForteError = forteError;
        }

    }
}