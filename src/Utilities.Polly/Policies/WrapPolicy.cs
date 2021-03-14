using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Wrap;

namespace Utilities.Polly.Policies
{
    public class WrapPolicy : IPolicySetup
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
          return  Policy.WrapAsync(_fallBackPolicyAsync, _circuitBreakerPolicyAsync, _retryPolicyAsync, _timeOutPolicyAsync,
                _bulkheadPolicyAsync);
        }

        //public IAsyncPolicy<HttpResponseMessage> TimeOutPolicyAsync { get; set; }
        //public IAsyncPolicy<HttpResponseMessage> HttpRetryPolicyAsync { get; set; }
        //public IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicyAsync { get; set; }
        //public IAsyncPolicy<HttpResponseMessage> FallBackPolicyAsync { get; set; }
        //public IAsyncPolicy<HttpResponseMessage> BulkheadPolicyAsync { get; set; }
        public AsyncPolicyWrap<HttpResponseMessage> AllPolicyWrapAsync { get; set; }

        //private IAsyncPolicy<HttpResponseMessage> SetupTimeoutPolicy()
        //{
        //    //todo:configure it somewhere
        //    return Policy.TimeoutAsync<HttpResponseMessage>(5);
        //}

        //private IAsyncPolicy<HttpResponseMessage> SetupRetryPolicy()
        //{
        //    return Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).Or<TimeoutRejectedException>()
        //        .OrResult(r => _httpStatusCodesWorthRetrying.Contains(r.StatusCode))
        //        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt), OnRetry);
        //}

        //private IAsyncPolicy<HttpResponseMessage> SetupCircuitBreakerPolicy()
        //{
        //    //todo:configure it somewhere
        //    return Policy
        //        .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
        //        .AdvancedCircuitBreakerAsync(0.5, TimeSpan.FromSeconds(60), 7, TimeSpan.FromSeconds(15), OnBreak,
        //            OnReset, OnHalfOpen);
        //}

        //Enables us to send some meaningful defaults if all else has failed so it will be the last line of defense.
        //The Fallback policy allows us to perform any action when a request fails, this could be restarting a service,
        //messaging an admin, scaling out a cluster, etc. It is usually used as at the last policy to be called inside a policy wrap.
        //private IAsyncPolicy<HttpResponseMessage> SetupFallBackPolicy()
        //{
        //    return Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).Or<TimeoutRejectedException>()
        //        .FallbackAsync(FallbackAction, OnFallbackAsync); 
        //}
        //private Task OnFallbackAsync(DelegateResult<HttpResponseMessage> response, Context context)
        //{
        //    _logger.LogWarning($"OnFallbackAsync is executing");
        //    var msg = GenerateLogMessageFromContext(context);
        //   _logger.LogWarning(msg);
        //    return Task.CompletedTask;
        //}

        ////Todo:As a team we should decide what decision should be taken here!
        //private Task<HttpResponseMessage> FallbackAction(DelegateResult<HttpResponseMessage> responseToFailedRequest, Context context, CancellationToken cancellationToken)
        //{
        //    Console.WriteLine("Fallback action is executing");

        //    var httpResponseMessage = new HttpResponseMessage(responseToFailedRequest.Result.StatusCode)
        //    {
        //        Content = new StringContent($"The fallback executed, the original error was {responseToFailedRequest.Result.ReasonPhrase}")
        //    };
        //    return Task.FromResult(httpResponseMessage);
        //}
        //This policy would help keep the application stable under a heavy load or when a service it relies on is responding too slowly or has gone down
        ////It can be used also for load shedding or a trigger for horizontal scaling 
        //private IAsyncPolicy<HttpResponseMessage> SetupBulkheadPolicy()
        //{
        //    //todo:configure it somewhere
        //    return Policy.BulkheadAsync<HttpResponseMessage>(2, 4, OnBulkheadRejectedAsync);
        //}

        //private void OnRetry(DelegateResult<HttpResponseMessage> exception, TimeSpan timeSpan, int retryCount,
        //    Context context)
        //{
        //    //Logging such information would be helpful to understand what is happening with the consumer.
        //    var msg = $"Retry {retryCount} ";
        //    msg += GenerateLogMessageFromContext(context);
        //    _logger.LogWarning(msg);
        //}

        //private static string GenerateLogMessageFromContext(Context context)
        //{
        //    var msg = $"of {context.PolicyKey} " +
        //              $"at {context.PolicyWrapKey}, ";
        //    if (context.ContainsKey("Host")) msg += $"Host:{context["Host"]} ";

        //    if (context.ContainsKey("User-Agent")) msg += $"User-Agent:{context["User-Agent"]}";
        //    return msg;
        //}

        //private void OnHalfOpen()
        //{
        //    _logger.LogInformation("Connection half open");
        //}

        //private void OnReset()
        //{
        //    _logger.LogInformation("Connection reset");
        //}

        //private void OnBreak(DelegateResult<HttpResponseMessage> delegateResult, TimeSpan timeSpan)
        //{
        //    _logger.LogWarning($"Connection break: {delegateResult.Result}, {timeSpan}");
        //}

        //private Task OnBulkheadRejectedAsync(Context arg)
        //{
        //    //Todo:As a team we should decide what decision should be taken here:
        //    //( ex: scaling out or shed load quickly) so we need to find a balance
        //    //between protecting the application and allowing it to perform close to the limit of its ability
        //    _logger.LogWarning("OnBulkheadRejectedAsync Executed");
        //    return Task.CompletedTask;
        //}
    }
}