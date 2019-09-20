using System.Net.Http;
using System.Threading.Tasks;

namespace ApiAndClient.Interfaces
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage);
    }
}
