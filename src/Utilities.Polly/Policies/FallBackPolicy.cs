using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;

namespace Utilities.Polly.Policies
{
    public class FallBackPolicy:IPolicySetup
    {
        private static ILogger _logger;
        public FallBackPolicy(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }
        public IAsyncPolicy<HttpResponseMessage> PolicyAsync => Setup();
        private IAsyncPolicy<HttpResponseMessage> Setup()
        {
            return Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).Or<TimeoutRejectedException>()
                .FallbackAsync(FallbackAction, OnFallbackAsync);
        }

        private Task OnFallbackAsync(DelegateResult<HttpResponseMessage> response, Context context)
        {
            _logger.LogWarning("OnFallbackAsync is executing");
            var msg = PolicySharedData.GenerateLogMessageFromContext(context);
            _logger.LogWarning(msg);
            return Task.CompletedTask;
        }

        //Todo:As a team we should decide what decision should be taken here!
        private Task<HttpResponseMessage> FallbackAction(
            DelegateResult<HttpResponseMessage> responseToFailedRequest, Context context,
            CancellationToken cancellationToken)
        {
            Console.WriteLine("Fallback action is executing");

            var httpResponseMessage = new HttpResponseMessage(responseToFailedRequest.Result.StatusCode)
            {
                Content = new StringContent(
                    $"The fallback executed, the original error was {responseToFailedRequest.Result.ReasonPhrase}")
            };
            return Task.FromResult(httpResponseMessage);
        }
    }
}