using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAndClient.Entities;
using BridgePacketRateLimiterApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiAndClient.Repositories.SqlRepository
{
    public class ApiRepository : IApiRepository
    {
        private readonly IApiRepositoryContextFactory _contextFactory;
        private ApiRepositoryContext _context;

        public ApiRepository(IApiRepositoryContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _context = _contextFactory.Build();
        }

        public void ResetContext()
        {
            _context = _contextFactory.Build();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<ResourceEntity>> GetAllResourcesAsync()
        {
            var resources = await _context
                .Resources
                .Include(b => b.Messages)
                .ToListAsync();
            return resources;
        }

        public Task<List<ResourceEntity>> GetAllMatureResourcesAsync(long now)
        {
            var resources = _context
                .Resources
                .OrderBy(e => e.NextTimeInMilliseconds)
                .Include(b => b.Messages)
                .Where
                (t => 
                    (t.NextTimeInMilliseconds < now && t.Tag == null) ||
                    (t.TagOrphanedTime != 0 && t.TagOrphanedTime < now)
                )
                .ToListAsync();
            return resources;
        }

        public async Task<List<ResourceEntity>> GetTaggedResourcesAsync(string tag)
        {
            var resources = await _context
                .Resources
                .OrderBy(e => e.NextTimeInMilliseconds)
                .Include(r => r.Messages)
                .Where(t => t.Tag == tag)
                .ToListAsync();
            return resources;
        }

        public async Task<ResourceEntity> GetResourceAsync(long resourceId)
        {
            var resource = await _context
                .Resources
                .Include(b => b.Messages)
                .Where(b => b.ResourceId == resourceId)
                .FirstOrDefaultAsync();
            return resource;
        }

        public void AddResource(ResourceEntity resource)
        {
            _context.Add(resource);
        }

        public void DeleteResource(ResourceEntity resource)
        {
            _context.Remove(resource);
        }

        public async Task<List<MessageEntity>> GetAllMessagesAsync()
        {
            var resources = await _context
                .Messages
                .ToListAsync();
            return resources;
        }
    }
}
