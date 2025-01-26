using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ConfigServer.Domain.Entities;
using ConfigServer.Domain.Interfaces;

namespace ConfigServer.Web.Controllers
{  
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigRepository _configRepository;
        private readonly IUserContextService _userContextService;

        public ConfigController(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
            _userContextService = userContextService
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Config>>> GetAllConfigs()
        {
            var configs = await _configRepository.GetAllAsync();
            return Ok(configs);
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Config>> GetConfigById(Guid id)
        {
            var config = await _configRepository.GetByIdAsync(id);
            if (config == null)
            {
                return NotFound(new { message = "Configuration not found." });
            }

            return Ok(config);
        }

        
        [HttpPost]
        public async Task<ActionResult> CreateConfig([FromBody] Config config)
        {
            if (config == null || string.IsNullOrWhiteSpace(config.Value)|| string.IsNullOrWhiteSpace(config.Key))
            {
                return BadRequest(new { message = "Invalid configuration data." });
            }

            var userId = _userContextService.GetCurrentUserId();
            config.AuthorId = userId;

            await _configRepository.AddAsync(config);
            await _configRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConfigById), new { id = config.Id }, config);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateConfig(Guid id, [FromBody] Config updatedConfig)
        {
            var existingConfig = await _configRepository.GetByIdAsync(id);
            if (existingConfig == null)
            {
                return NotFound(new { message = "Configuration not found." });
            }

            existingConfig.Key = updatedConfig.Key;
            existingConfig.Value = updatedConfig.Value;
            existingConfig.Description = updatedConfig.Description;
            existingConfig.UpdatedAt = DateTime.UtcNow;

            await _configRepository.UpdateAsync(existingConfig);
            await _configRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}