using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAndClient.Entities;
using ApiAndClient.Interfaces;
using ApiAndClient.Repositories;
using BridgePacketRateLimiterApi.Entities;
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

        public async Task AddMessageAsync(long resourceId, MessageEntity message, int retryCount = 0)
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
                if (retryCount > MaxRetries)
                {
                    throw;
                }
                else
                {
                    _repository.ResetContext();
                    message.Id = 0;
                    await AddMessageAsync(resourceId, message, ++retryCount);
                }
            }
        }

        public async Task<List<MessageEntity>> GetMessagesToSendAsync(int retryCount = 0)
        {
            List<MessageEntity> response = null;
            try
            {
                _logger.LogDebug("Start GetMessagesToSendAsync");
                var now = GetNowInMilliseconds();
                var tagOrphanedTime = now + TagOrphanDelayInMilliseconds;
                var tag = GetTag();
                response = new List<MessageEntity>();
                var resources = (await _repository.GetAllMatureResourcesAsync(now)).ToList();
                if (resources.Count > 0)
                {
                    foreach (var resource in resources)
                    {
                        resource.Tag = tag;
                        resource.TagOrphanedTime = tagOrphanedTime;
                    }

                    await _repository.SaveChangesAsync();

                    // if we are here, then we have a coherent list of resources to process for sending              
                    _repository.ResetContext();
                    var taggedResources = (await _repository.GetTaggedResourcesAsync(tag)).ToList();

                    foreach (var resource in taggedResources)
                    {
                        var sendable = resource.GetNextMessageToSend();
                        if (resource.Messages.Count == 0)
                        {
                            _repository.DeleteResource(resource);
                        }
                        else
                        {
                            resource.NextTimeInMilliseconds = GetNowInMilliseconds() + ResourceEntity.DefaultWaitTimeInMilliseconds;
                        }

                        response.Add(sendable);
                    }

                    foreach (var resource in taggedResources)
                    {
                        resource.ClearTags();
                    }

                    await _repository.SaveChangesAsync();
                    _logger.LogDebug("End GetMessagesToSendAsync");
                }
            }
            catch (DbUpdateException) // Concurrency, Uniqueness, or other data exception
            {
                _logger.LogWarning("DbUpdateException GetMessagesToSendAsync");
                _repository.ResetContext();                
                if (retryCount > MaxRetries)
                {
                    throw;
                }
                else
                {
                    await GetMessagesToSendAsync(++retryCount);
                }
            }

            return response;
        }
    }
}
