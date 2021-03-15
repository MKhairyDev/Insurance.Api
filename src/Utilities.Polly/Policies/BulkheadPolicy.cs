using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;

namespace Utilities.Polly.Policies
{
    public  class BulkheadPolicy: IPolicyWrapper
    {
        private static ILogger _logger;
        public BulkheadPolicy(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }
        public IAsyncPolicy<HttpResponseMessage> PolicyAsync => Setup();
        private  IAsyncPolicy<HttpResponseMessage> Setup()
        {
            //todo:configure it somewhere
            return Policy.BulkheadAsync<HttpResponseMessage>(maxParallelization:2,maxQueuingActions: 4, OnBulkheadRejectedAsync);
        }

        private  Task OnBulkheadRejectedAsync(Context arg)
        {
            //Todo:As a team we should decide what decision should be taken here:
            //( ex: scaling out or shed load quickly) so we need to find a balance
            //between protecting the application and allowing it to perform close to the limit of its ability
            _logger.LogWarning("OnBulkheadRejectedAsync Executed");
            return Task.CompletedTask;
        }


    }
}