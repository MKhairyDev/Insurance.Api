using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Polly;

namespace Utilities.Polly.Policies
{
    public class CircuitBreakerPolicy: IPolicySetup
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
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .AdvancedCircuitBreakerAsync(failureThreshold: 0.5, samplingDuration: TimeSpan.FromSeconds(60),
                    minimumThroughput: 7, durationOfBreak: TimeSpan.FromSeconds(15), OnBreak,
                    OnReset, OnHalfOpen);
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