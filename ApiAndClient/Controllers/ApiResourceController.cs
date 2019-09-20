using System.Threading.Tasks;
using ApiAndClient.Entities;
using ApiAndClient.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiAndClient.Controllers
{
    [Authorize(Policy = "User")]
    [Authorize("Bearer")]
    [Route("api/")]
    public class ApiResourceController : ControllerBase
    {
        private readonly IApiService _service;
        private readonly ILogger<ApiResourceController> _logger;

        public ApiResourceController
        (
            IApiService service,
            ILogger<ApiResourceController> logger
        )
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("messages")]
        public async Task<IActionResult> GetMessagesAsync()
        {
            _logger.LogDebug("Start GetAllMessagesAsync");
            var resources = await _service.GetAllMessagesAsync();
            _logger.LogInformation("End GetAllMessagesAsync");
            return Ok(resources); 
        }

        [HttpGet("resources")]
        public async Task<IActionResult> GetResourcesAsync()
        {
            _logger.LogDebug("Start GetAllResourcesAsync");
            var resources = await _service.GetAllResourcesAsync();
            _logger.LogInformation("End GetAllResourcesAsync");
            return Ok(resources);
        }

        [HttpPost("resources/{resourceId}/messages")]
        public async Task<IActionResult> AddMessageAsync(long resourceId, [FromBody] MessageEntity message)
        {
            _logger.LogDebug($"ResourceId: {resourceId} | Start AddMessageAsync");
            await _service.AddMessageAsync(resourceId, message);
            _logger.LogInformation($"ResourceId: {resourceId} | End AddMessageAsync");
            return NoContent();
        }

        [HttpPost("resources")]
        public async Task<IActionResult> AddResourceAsync([FromBody] ResourceEntity resource)
        {
            _logger.LogDebug($"Start AddResourceAsync");
            await _service.AddResourceAsync(resource);
            _logger.LogInformation($"ResourceId: {resource.ResourceId} | End AddResourceAsync");
            return NoContent();
        }


    }
}
