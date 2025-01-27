using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ConfigServer.Domain.Entities;
using ConfigServer.Application.DTOs;
using ConfigServer.Domain.Interfaces;
using ConfigServer.Infrastructure.Services;

namespace ConfigServer.Web.Controllers
{  
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigRepository _configRepository;
        private readonly TokenHelper _tokenHelper;
        public ConfigController(IConfigRepository configRepository, TokenHelper tokenHelper)
        {
            _configRepository = configRepository;
            _tokenHelper = tokenHelper;
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
   public async Task<ActionResult> CreateConfig([FromBody] ConfigDTO configDto)
{
    if (configDto == null || string.IsNullOrWhiteSpace(configDto.Value) || string.IsNullOrWhiteSpace(configDto.Key))
    {
        return BadRequest(new { message = "Invalid configuration data." });
    }

    try
    {
        
        var (userId, userRole) = _tokenHelper.GetUserFromCookie();

       
        if (userRole != "Admin" && userRole != "Editor") 
        {
            return Unauthorized(new { message = "You do not have permission to create configurations." });
        }

        
        var config = new Config
        {
            Id = Guid.NewGuid(),
            Key = configDto.Key,
            Value = configDto.Value,
            Description = configDto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UserId = userId 
        };

        
        await _configRepository.AddAsync(config);
        await _configRepository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetConfigById), new { id = config.Id }, config);
    }
    catch (UnauthorizedAccessException ex)
    {
        return Unauthorized(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
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