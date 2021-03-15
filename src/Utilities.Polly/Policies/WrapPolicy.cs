using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Wrap;

namespace Utilities.Polly.Policies
{
    public class WrapPolicy : IPolicyWrapper
    {
        private readonly IAsyncPolicy<HttpResponseMessage> _timeOutPolicyAsync;
        private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicyAsync;
        private readonly IAsyncPolicy<HttpResponseMessage> _circuitBreakerPolicyAsync;
        private readonly IAsyncPolicy<HttpResponseMessage> _fallBackPolicyAsync;
        private readonly IAsyncPolicy<HttpResponseMessage> _bulkheadPolicyAsync;

        public WrapPolicy(ILogger logger,
            IAsyncPolicy<HttpResponseMessage> bulkheadPolicyAsync,
            IAsyncPolicy<HttpResponseMessage> timeOutPolicyAsync,
            IAsyncPolicy<HttpResponseMessage> retryPolicyAsync,
            IAsyncPolicy<HttpResponseMessage> circuitBreakerPolicyAsync,
            IAsyncPolicy<HttpResponseMessage> fallBackPolicyAsync)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            _timeOutPolicyAsync = timeOutPolicyAsync;
            _retryPolicyAsync = retryPolicyAsync;
            _circuitBreakerPolicyAsync = circuitBreakerPolicyAsync;
            _fallBackPolicyAsync = fallBackPolicyAsync;
            _bulkheadPolicyAsync = bulkheadPolicyAsync;

        }

        public IAsyncPolicy<HttpResponseMessage> PolicyAsync => Setup();

        private IAsyncPolicy<HttpResponseMessage> Setup()
        {
            return Policy.WrapAsync(_fallBackPolicyAsync, _circuitBreakerPolicyAsync, _retryPolicyAsync,
                _timeOutPolicyAsync,
                _bulkheadPolicyAsync);
        }
    }
}