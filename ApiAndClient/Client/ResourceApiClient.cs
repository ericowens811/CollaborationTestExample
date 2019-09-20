using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ApiAndClient.Entities;
using ApiAndClient.Interfaces;
using BridgePacketRateLimiterApi.Entities;
using Newtonsoft.Json;

namespace ApiAndClient.Client
{
    public class ResourceApiClient
    {
        private readonly IHttpClient _client;
        private readonly IRequestBuilder _requestBuilder;
        private readonly IResourceApiUrls _resourceUrls;

        public ResourceApiClient
        (
            IHttpClient client,
            IRequestBuilder requestBuilder,
            IResourceApiUrls resourceUrls
        )
        {
            _client = client;
            _requestBuilder = requestBuilder;
            _resourceUrls = resourceUrls;
        }

        public async Task AddMessageAsync(MessageEntity message)
        {
            var endpoint = $"{_resourceUrls.ResourceUrl}/{message.ResourceId}/messages";
            var request = _requestBuilder.Build(HttpMethod.Post, endpoint, message);
            await _client.SendAsync(request);
        }

        public async Task<List<MessageEntity>> GetMessagesToSendAsync()
        {
            var endpoint = $"{_resourceUrls.ResourceUrl}/messages";
            var request = _requestBuilder.Build(HttpMethod.Get, endpoint);
            var response = await _client.SendAsync(request);
            var contentJson = await response.Content.ReadAsStringAsync();
            var data =  JsonConvert.DeserializeObject<List<MessageEntity>>(contentJson);
            return data;
        }

        public async Task<List<ResourceEntity>> GetAllResourcesAsync()
        {
            var request = _requestBuilder.Build(HttpMethod.Get, _resourceUrls.ResourceUrl);
            var response = await _client.SendAsync(request);
            var contentJson = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<ResourceEntity>>(contentJson);
            return data;
        }
    }
}
