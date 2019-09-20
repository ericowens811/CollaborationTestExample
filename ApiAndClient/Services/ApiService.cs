using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAndClient.Entities;
using ApiAndClient.Interfaces;
using ApiAndClient.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiAndClient.Services
{
    public class ApiService : IApiService
    {
        public const int MaxRetries = 3;
        public const long TagOrphanDelayInMilliseconds = 10000;

        private readonly IApiRepository _repository;
        private readonly INowProvider _nowProvider;
        private readonly ITagProvider _tagProvider;
        private readonly ILogger<ApiService> _logger;

        public ApiService
        (
            IApiRepository repository,
            INowProvider nowProvider,
            ITagProvider tagProvider,
            ILogger<ApiService> logger
        )
        {
            _repository = repository;
            _nowProvider = nowProvider;
            _tagProvider = tagProvider;
            _logger = logger;
        }

        private long GetNowInMilliseconds()
        {
            return _nowProvider.GetNowInMillisecond();
        }

        private string GetTag()
        {
            return _tagProvider.NextTag();
        }

        public async Task<List<ResourceEntity>> GetAllResourcesAsync()
        {
            var result = await _repository.GetAllResourcesAsync();
            return result;
        }

        public async Task<List<MessageEntity>> GetAllMessagesAsync()
        {
            var result = await _repository.GetAllMessagesAsync();
            return result;
        }

        public async Task AddMessageAsync(long resourceId, MessageEntity message)
        {
            try
            {
                _logger.LogDebug($"ResourceId: {resourceId} | Start AddMessageAsync");
                var resource = await _repository.GetResourceAsync(resourceId);
                if (resource == null)
                {
                    resource = new ResourceEntity(resourceId);
                    _repository.AddResource(resource);
                }

                resource.AddMessage(message, GetNowInMilliseconds());

                await _repository.SaveChangesAsync();
                _logger.LogDebug($"ResourceId: {resourceId} | End AddMessageAsync");
            }
            catch (DbUpdateException) // Concurrency, Uniqueness, or other data exception
            {
                _logger.LogWarning($"ResourceId: {resourceId} | DbUpdateException AddMessageAsync");
            }
        }

        public async Task AddResourceAsync(ResourceEntity resource)
        {
            try
            {
                _logger.LogDebug("Start AddResourceAsync");
                _repository.AddResource(resource);
                await _repository.SaveChangesAsync();
                _logger.LogDebug("End AddResourceAsync");
            }
            catch (DbUpdateException) // Concurrency, Uniqueness, or other data exception
            {
                _logger.LogWarning($"DbUpdateException AddResourceAsync");
            }
        }
    }
}
