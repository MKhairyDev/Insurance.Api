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

            AddPoliciesToRegistry(services);

            return services;
        }

        private static void AddPoliciesToRegistry(IServiceCollection services)
        {
            var logger = new LoggerFactory().CreateLogger("polly_policy");
            var policyFactory = new PolicyFactory(logger);
            var registry = services.AddPolicyRegistry();

            registry.Add(PolicyName.Bulkhead.ToString(), policyFactory.GetPolicy(PolicyName.Bulkhead));
            registry.Add(PolicyName.Retry.ToString(), policyFactory.GetPolicy(PolicyName.Retry));
            registry.Add(PolicyName.CircuitBreaker.ToString(), policyFactory.GetPolicy(PolicyName.CircuitBreaker));
            registry.Add(PolicyName.WrapAll.ToString(), policyFactory.GetPolicy(PolicyName.WrapAll));
            registry.Add(PolicyName.NoOpAsync.ToString(), policyFactory.GetPolicy(PolicyName.NoOpAsync));
        }
    }
}