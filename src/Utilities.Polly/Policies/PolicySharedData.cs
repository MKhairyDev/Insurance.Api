using System.Collections.Generic;
using System.Net;
using Polly;

namespace Utilities.Polly.Policies
{
   public static class PolicySharedData
    {
        // Handle both exceptions and return values in one policy
        public static List<HttpStatusCode> HttpStatusCodesWorthRetrying { get; } = new List<HttpStatusCode>
        {
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.BadGateway,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout
        };
        public static string GenerateLogMessageFromContext(Context context)
        {
            var msg = $"of {context.PolicyKey} " +
                      $"at {context.PolicyWrapKey}, " +
                      $"for CorrelationId: {context.CorrelationId}, ";
            return msg;
        }
    }
}
