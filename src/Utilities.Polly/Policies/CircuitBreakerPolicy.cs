using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;

namespace Utilities.Polly.Policies
{
    public class CircuitBreakerPolicy: IPolicyWrapper
    {
        private static ILogger _logger;
        public CircuitBreakerPolicy(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }
        public IAsyncPolicy<HttpResponseMessage> PolicyAsync => Setup();
        private IAsyncPolicy<HttpResponseMessage> Setup()
        {
            //todo:configure it somewhere
            return Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).Or<TimeoutRejectedException>().Or<HttpRequestException>()
                .OrResult(r => PolicySharedData.HttpStatusCodesWorthRetrying.Contains(r.StatusCode)).CircuitBreakerAsync(2, TimeSpan.FromSeconds(60), OnBreak,
              OnReset, OnHalfOpen);

            /*
             You might use this Advanced Circuit Breaker when you know that some percentage of requests will be lost, but you know your application can tolerate it. Or,
             when you have bursty traffic and a few consecutive errors don’t indicate a serious fault.   
            ex: AdvancedCircuitBreakerAsync(failureThreshold: 0.5, samplingDuration: TimeSpan.FromSeconds(60),
            minimumThroughput: 7, durationOfBreak: TimeSpan.FromSeconds(15), OnBreak, OnReset, OnHalfOpen);
            */
        }

        private void OnHalfOpen()
        {
            _logger.LogInformation("Connection half open");
        }

        private void OnReset()
        {
            _logger.LogInformation("Connection reset");
        }

        private void OnBreak(DelegateResult<HttpResponseMessage> delegateResult, TimeSpan timeSpan)
        {
            _logger.LogWarning($"Connection break: {delegateResult.Result}, {timeSpan}");
        }


    }
}