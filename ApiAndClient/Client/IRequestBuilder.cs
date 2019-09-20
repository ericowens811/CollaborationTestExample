using System.Net.Http;

namespace ApiAndClient.Client
{
    public interface IRequestBuilder
    {
        HttpRequestMessage Build(HttpMethod method, string endpoint, object content = null);
    }
}
