using System.ComponentModel;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Polly;

namespace Utilities.Polly.Policies
{
    public enum PolicyName
    {
        Bulkhead,
        Timeout,
        Retry,
        CircuitBreaker,
        FallBack,
        WrapAll,
        NoOpAsync

    }
   public  class PolicyFactory
    {
        private readonly ILogger _logger;

        public PolicyFactory(ILogger logger)
        {
            _logger = logger;
        }
        public IAsyncPolicy<HttpResponseMessage> GetPolicy(PolicyName policyName)
        {
            switch (policyName)
            {
                case PolicyName.Bulkhead:
                    return new BulkheadPolicy(_logger).PolicyAsync;
                case PolicyName.Timeout:
                    return new TimeoutPolicy().PolicyAsync;
                case PolicyName.Retry:
                    return new RetryPolicy(_logger).PolicyAsync;
                case PolicyName.CircuitBreaker:
                    return new CircuitBreakerPolicy(_logger).PolicyAsync;
                case PolicyName.FallBack:
                    return new FallBackPolicy(_logger).PolicyAsync;
                case PolicyName.WrapAll:
                    return new WrapPolicy(_logger, new BulkheadPolicy(_logger).PolicyAsync,
                        new TimeoutPolicy().PolicyAsync,
                        new RetryPolicy(_logger).PolicyAsync, new CircuitBreakerPolicy(_logger).PolicyAsync,
                        new FallBackPolicy(_logger).PolicyAsync).PolicyAsync;
                case PolicyName.NoOpAsync:
                    return new NoOpAsyncPolicy().PolicyAsync;
                default:
                    throw new InvalidEnumArgumentException(nameof(policyName), null);
            }
        }
    }
}
