using System;
using System.Runtime.Serialization;

namespace Utilities.Polly.Exceptions
{
    [Serializable]
    public class UnauthorizedApiAccessException : Exception
    {
        public UnauthorizedApiAccessException()
        {
        }

        public UnauthorizedApiAccessException(string message) : base(message)
        {
        }

        public UnauthorizedApiAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnauthorizedApiAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}