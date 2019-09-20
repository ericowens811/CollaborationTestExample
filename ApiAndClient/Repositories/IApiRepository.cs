using System.Collections.Generic;
using System.Threading.Tasks;
using ApiAndClient.Entities;
using BridgePacketRateLimiterApi.Entities;

namespace ApiAndClient.Repositories
{
    public interface IApiRepository
    {
        Task SaveChangesAsync();
        Task<List<ResourceEntity>> GetAllResourcesAsync();
        Task<List<ResourceEntity>> GetAllMatureResourcesAsync(long now);
        Task<List<ResourceEntity>> GetTaggedResourcesAsync(string tag);
        Task<ResourceEntity> GetResourceAsync(long resourceId);
        void AddResource(ResourceEntity resource);
        void DeleteResource(ResourceEntity resource);
        void ResetContext();
        Task<List<MessageEntity>> GetAllMessagesAsync();
    }
}
