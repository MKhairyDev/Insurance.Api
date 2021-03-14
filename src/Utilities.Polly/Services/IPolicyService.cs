//using System.Net.Http;
//using Polly;
//using Polly.Wrap;

//namespace Utilities.Polly.Services
//{
//    public interface IPolicyService
//    {
//        IAsyncPolicy<HttpResponseMessage> TimeOutPolicyAsync { get; }
//        IAsyncPolicy<HttpResponseMessage> HttpRetryPolicyAsync { get; }
//        IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicyAsync { get; }
//        IAsyncPolicy<HttpResponseMessage> FallBackPolicyAsync { get;}
//        IAsyncPolicy<HttpResponseMessage> BulkheadPolicyAsync { get; }

//        AsyncPolicyWrap<HttpResponseMessage> AllPolicyWrapAsync { get;}
//    }

//  public  class PolicyService : IPolicyService
//    {
//        public IAsyncPolicy<HttpResponseMessage> TimeOutPolicyAsync { get; }
//        public IAsyncPolicy<HttpResponseMessage> HttpRetryPolicyAsync { get; }
//        public IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicyAsync { get; }
//        public IAsyncPolicy<HttpResponseMessage> FallBackPolicyAsync { get; }
//        public IAsyncPolicy<HttpResponseMessage> BulkheadPolicyAsync { get; }
//        public AsyncPolicyWrap<HttpResponseMessage> AllPolicyWrapAsync { get; }
//    }
//}