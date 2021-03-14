using System.Net.Http;
using Polly;

namespace Utilities.Polly.Policies
{
    public class NoOpAsyncPolicy : IPolicySetup
    {
        public IAsyncPolicy<HttpResponseMessage> PolicyAsync => Setup();
        private static IAsyncPolicy<HttpResponseMessage> Setup()
        {
            return Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();
        }
    }
}