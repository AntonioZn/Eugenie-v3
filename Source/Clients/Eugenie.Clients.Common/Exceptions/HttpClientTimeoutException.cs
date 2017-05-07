namespace Eugenie.Clients.Common.Exceptions
{
    using System;

    public class HttpClientTimeoutException : Exception
    {
        public HttpClientTimeoutException(string message) : base(message)
        {
            
        }
    }
}
