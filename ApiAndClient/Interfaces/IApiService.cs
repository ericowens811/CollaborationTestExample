using System.Collections.Generic;
using System.Threading.Tasks;
using ApiAndClient.Entities;
using BridgePacketRateLimiterApi.Entities;

namespace ApiAndClient.Interfaces
{
    public interface IApiService
    {
        Task<List<ResourceEntity>> GetAllResourcesAsync();
        Task AddMessageAsync(long resourceId, MessageEntity message, int retryCount = 0);
        Task<List<MessageEntity>> GetMessagesToSendAsync(int retryCount = 0);
    }
}
