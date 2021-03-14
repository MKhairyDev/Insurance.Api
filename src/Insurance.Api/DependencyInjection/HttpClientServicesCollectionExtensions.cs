using System.Net.Http;
using Insurance.Api.External;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;
using Utilities.Polly.Policies;

namespace Insurance.Api.DependencyInjection
{
    public static class HttpClientServicesCollectionExtensions
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {

            services.AddHttpClient<IProductApiClient, ProductApiClient>().AddPolicyHandlerFromRegistry((PolicySelector)); 

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> PolicySelector(IReadOnlyPolicyRegistry<string> policyRegistry,
            HttpRequestMessage httpRequestMessage)
        {
            if (httpRequestMessage.Method == HttpMethod.Post)
            {
                return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(PolicyName.NoOpAsync.ToString());
            }

            return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(PolicyName.WrapAll.ToString());
        }
    }
}