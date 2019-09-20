using System.Collections.Generic;
using System.Threading.Tasks;
using ApiAndClient.Entities;

namespace ApiAndClient.Repositories
{
    public interface IApiRepository
    {
        Task SaveChangesAsync();
        Task<List<ResourceEntity>> GetAllResourcesAsync();
        Task<ResourceEntity> GetResourceAsync(long resourceId);
        void AddResource(ResourceEntity resource);
        void DeleteResource(ResourceEntity resource);
        Task<List<MessageEntity>> GetAllMessagesAsync();
        void ResetContext();
    }
}
