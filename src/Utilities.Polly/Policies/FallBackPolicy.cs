using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;

namespace Utilities.Polly.Policies
{
    public class FallBackPolicy : IPolicyWrapper
    {
        private static ILogger _logger;

        public FallBackPolicy(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
        }

        public IAsyncPolicy<HttpResponseMessage> PolicyAsync => Setup();

        private IAsyncPolicy<HttpResponseMessage> Setup()
        {
            return Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).Or<TimeoutRejectedException>()
                .Or<HttpRequestException>()
                .OrResult(r => PolicySharedData.HttpStatusCodesWorthRetrying.Contains(item: r.StatusCode))
                .FallbackAsync(fallbackAction: FallbackAction, onFallbackAsync: OnFallbackAsync);
        }

        private Task OnFallbackAsync(DelegateResult<HttpResponseMessage> response, Context context)
        {
            _logger.LogWarning("OnFallbackAsync is executing");
            var msg = "";
            msg += response.Exception?.ToString() ?? response.Result?.ToString();
            msg += PolicySharedData.GenerateLogMessageFromContext(context: context);
            _logger.LogWarning(message: msg);
            return Task.CompletedTask;
        }

        //Todo:As a team we should decide what decision should be taken here!
        private Task<HttpResponseMessage> FallbackAction(
            DelegateResult<HttpResponseMessage> responseToFailedRequest, Context context,
            CancellationToken cancellationToken)
        {
            _logger.LogWarning("Fallback action is executing");
            var statusCode = responseToFailedRequest?.Result?.StatusCode ?? HttpStatusCode.InternalServerError;
            var reasonPhrase = responseToFailedRequest?.Result?.ReasonPhrase ?? "Internal Server Error";
            var msg = $"The fallback executed, the original error was {reasonPhrase}";
            var httpResponseMessage = new HttpResponseMessage(statusCode: statusCode)
            {
                Content = new StringContent(content: msg)
            };
            _logger.LogWarning(message: msg);
            return Task.FromResult(result: httpResponseMessage);
        }
    }
}