using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using CollaborationTestExample.Jwt.Authentication;
using Newtonsoft.Json;

namespace ApiAndClient.Client
{
    public class RequestBuilder : IRequestBuilder
    {
        private const string ApplicationJson = "application/json";
        private readonly IProvideJwt _jwtProvider;
        public RequestBuilder(IProvideJwt jwtProvider)
        {
            _jwtProvider = jwtProvider;
        }

        public HttpRequestMessage Build(HttpMethod method, string endpoint, object content = null)
        {
            var request = new HttpRequestMessage(method, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _jwtProvider.GetJsonWebToken());
            if (content != null)
            {
                var contentAsJson = JsonConvert.SerializeObject(content);
                request.Content = new StringContent(contentAsJson, Encoding.UTF8, ApplicationJson);
            }
            return request;
        }
    }
}
