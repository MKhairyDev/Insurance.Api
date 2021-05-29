using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Insurance.Api.UnitTests.Handlers
{
    public class OkResponseHandler<T> : HttpMessageHandler where T: new()
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var json = JsonConvert.SerializeObject(new T());
            response.Content = new StringContent(json);
            return Task.FromResult(response);
        }
    }
}
