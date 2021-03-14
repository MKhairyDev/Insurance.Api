﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Insurance.Api.UnitTests.Handlers
{
    public class ResourceNotFoundResponseHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            return Task.FromResult(response);
        }
    }
}
