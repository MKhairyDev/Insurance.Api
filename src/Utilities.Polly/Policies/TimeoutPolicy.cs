using System.Net.Http;
using Polly;

namespace Utilities.Polly.Policies
{
    public class TimeoutPolicy: IPolicySetup
    {
        public IAsyncPolicy<HttpResponseMessage> PolicyAsync => Setup();
        private static IAsyncPolicy<HttpResponseMessage> Setup()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(5);
        }
    }
}