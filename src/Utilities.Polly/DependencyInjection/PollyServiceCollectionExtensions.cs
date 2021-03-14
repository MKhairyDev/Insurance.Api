using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using Utilities.Polly.Policies;

namespace Utilities.Polly.DependencyInjection
{
    public static class PollyServiceCollectionExtensions
    {
        public static IServiceCollection RegisterPollyPoliciesServices(this IServiceCollection services)
        {
            /*
             Polly is a library that allows developers to express resilience and transient fault handling policies such as Retry, Circuit Breaker, Timeout, Bulkhead Isolation, and Fallback in a fluent and thread-safe manner.
               
               Bulkhead isolation:
               It limits the number of  requests to the remote service that can execute in parallel and also limits the number of requests that can sit in a queue awaiting an execution slot 
               Benefits:
               Isolation
               Prevent overloading :
               If one part of your application becomes overloaded it could bring down other parts but by using Bulkhead isolation policy you could prevent this from happening or delay it.
               Resource allocation :
               By allocate execution slots and queues as you see fit for your case.
               Scaling :
               You can determine the number of ongoing parallel requests. You can also determine the number of requests waiting in the queue. When they reach some limit, you could trigger horizontal scaling.
               Load shedding :
               As it’s better to fail fast than to fail unpredictably so when your application is being overwhelmed, it will at some unknown point being slow and fail.
               So you could handle this by setting when your application stops accepting requests and immediately return an error to the caller
               
             */

            AddPoliciesToRegistry(services);

            return services;
        }

        private static void AddPoliciesToRegistry(IServiceCollection services)
        {
            var logger = new LoggerFactory().CreateLogger("policy");
            var policyFactory = new PolicyFactory(logger);
            IPolicyRegistry<string> registry = services.AddPolicyRegistry();

            registry.Add(PolicyName.Bulkhead.ToString(), policyFactory.GetPolicy(PolicyName.Bulkhead));
            registry.Add(PolicyName.Timeout.ToString(), policyFactory.GetPolicy(PolicyName.Timeout));
            registry.Add(PolicyName.Retry.ToString(), policyFactory.GetPolicy(PolicyName.Retry));
            registry.Add(PolicyName.CircuitBreaker.ToString(), policyFactory.GetPolicy(PolicyName.CircuitBreaker));
            registry.Add(PolicyName.FallBack.ToString(), policyFactory.GetPolicy(PolicyName.FallBack));
            registry.Add(PolicyName.WrapAll.ToString(), policyFactory.GetPolicy(PolicyName.WrapAll));
            registry.Add(PolicyName.NoOpAsync.ToString(), policyFactory.GetPolicy(PolicyName.NoOpAsync));
        }
    }
}