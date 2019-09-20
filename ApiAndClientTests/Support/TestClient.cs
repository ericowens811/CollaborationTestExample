using System.Net.Http;
using System.Threading.Tasks;
using ApiAndClient.Interfaces;

namespace ApiAndClientTests.Support
{
    public class TestClient : IHttpClient
    {
        private readonly HttpClient _client;
        public TestClient(HttpClient client)
        {
            _client = client;
        }
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            var response = await _client.SendAsync(requestMessage);
            return response;
        }
    }
}
