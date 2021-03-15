using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;

namespace Utilities.Polly.Policies
{
    public class RetryPolicy : IPolicyWrapper
    {
        private static ILogger _logger;

        public RetryPolicy(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
        }

        public IAsyncPolicy<HttpResponseMessage> PolicyAsync => Setup();

        private IAsyncPolicy<HttpResponseMessage> Setup()
        {
            return Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).Or<TimeoutRejectedException>()
                .Or<HttpRequestException>()
                .OrResult(r => PolicySharedData.HttpStatusCodesWorthRetrying.Contains(item: r.StatusCode))
                .WaitAndRetryAsync(retryCount: 3, retryAttempt => TimeSpan.FromSeconds(value: retryAttempt),
                    onRetry: OnRetry);
        }

        private void OnRetry(DelegateResult<HttpResponseMessage> exception, TimeSpan timeSpan, int retryCount,
            Context context)
        {
            //Logging such information would be helpful to understand what is happening with the consumer.
            var msg = $"Retry {retryCount} ";
            msg += PolicySharedData.GenerateLogMessageFromContext(context: context);
            _logger.LogWarning(message: msg);
        }
    }
}