using System.Collections.Generic;
using System.Threading.Tasks;
using ApiAndClient.Entities;

namespace ApiAndClient.Interfaces
{
    public interface IApiService
    {
        Task<List<ResourceEntity>> GetAllResourcesAsync();
        Task<List<MessageEntity>> GetAllMessagesAsync();
        Task AddMessageAsync(long resourceId, MessageEntity message);
        Task AddResourceAsync(ResourceEntity resource);
    }
}
