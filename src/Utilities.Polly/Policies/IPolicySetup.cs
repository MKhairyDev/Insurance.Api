using System.Net.Http;
using Polly;

namespace Utilities.Polly.Policies
{
    public interface IPolicySetup
    {
        IAsyncPolicy<HttpResponseMessage> PolicyAsync { get; }
    }
}
