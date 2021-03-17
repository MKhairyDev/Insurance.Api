using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Insurance.Api.Configuration;
using Insurance.Api.Extensions;
using Insurance.Api.Models;
using Microsoft.Extensions.Options;
using Utilities.Polly.Exceptions;

namespace Insurance.Api.External
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly HttpClient _client;
        private readonly CancellationTokenSource _cancellationTokenSource;
        public ProductApiClient(HttpClient client, 
            IOptionsMonitor<ExternalServicesConfig> options)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            var productApiConfig = options ?? throw new ArgumentNullException(nameof(options));
            _cancellationTokenSource = new CancellationTokenSource();
            var externalServicesConfig = productApiConfig.Get(ExternalServicesConfig.ProductApi);
            _client.BaseAddress = new Uri(externalServicesConfig.Url);
            _client.DefaultRequestHeaders.Clear();
        }

        public async Task<ProductDto> GetProductAsync(int productId)
        {
            //ToDo: Move this to a static class or configuration file
            string path = $"/products/{productId}";
            var httpRequestMsg = GenerateHttpGetRequestMessage(path);
            return await ExecuteHttpCallAsync<ProductDto>(httpRequestMsg, productId, _cancellationTokenSource.Token);

        }

        public Task<ProductTypeDto> GetProductTypeAsync(int productTypeId)
        {
            //ToDo: Move this to a static class or configuration file
            var path = $"/product_types/{productTypeId}";
            var httpRequestMsg = GenerateHttpGetRequestMessage(path);
            return ExecuteHttpCallAsync<ProductTypeDto>(httpRequestMsg, productTypeId, _cancellationTokenSource.Token);
        }

        private HttpRequestMessage GenerateHttpGetRequestMessage(string path)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, path);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpRequestMessage;
        }
        private async Task<T> ExecuteHttpCallAsync<T>(HttpRequestMessage httpRequestMessage,int id, CancellationToken cancellationToken)
        {

            /*
            Cancelling request could be beneficial to performance both at the level of bandwidth consumption and scalability
            Cancelling a task that is no longer needed will free up the thread that is used to run the task which mean that this thread will back to the thread pool
            which could be used for another work and this does improve the scalability of our application.
            "ResponseHeadersRead" is used blow instead the default option to be able to start the operation as soon as possible.
             */
            var response = await _client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);
            
                if (!response.IsSuccessStatusCode)
                {
                    // inspect the status code
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        throw new ResourceNotFoundException($"resource of type {typeof(T)} with an id {id} doesn't exist");
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        // trigger a login flow
                        throw new UnauthorizedApiAccessException();
                }

                /*When doing http request the response will be as a stream in a wire which could be read from the content as a string(ReadAsStringAsync)
                 which will create In memory string which as large as the response body, after that we need to transform that string by deserializing to the required object ,
                So what we can do, instead of deserializing this from that in memory object we could then deserializing it directly from the stream,
                which mean we don’t need such in memory string.thus we Ensure memory use is kept low and Minimizing memory can also minimize garbage collection,
                which has a positive impact on performance*/
                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);


                //Extension method has been created for re-usability aspect
                return stream.ReadAndDeserializeFromJson<T>();
            
        }
    }
}