using System.Net.Http;
using Polly;

namespace Utilities.Polly.Policies
{
    public class TimeoutPolicy: IPolicyWrapper
    {
        public IAsyncPolicy<HttpResponseMessage> PolicyAsync => Setup();
        private IAsyncPolicy<HttpResponseMessage> Setup()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(3);
        }
    }
}