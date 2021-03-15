using System.Net.Http;
using Polly;

namespace Utilities.Polly.Policies
{
    /// <summary>
    /// Wrapper for Polly asynchronous policy generic-typed,returning a ready for use policy with predefined configurations. 
    /// </summary>
    public interface IPolicyWrapper
    {
        IAsyncPolicy<HttpResponseMessage> PolicyAsync { get; }
    }
}
