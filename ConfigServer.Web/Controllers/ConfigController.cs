using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ConfigServer.Domain.Entities;
using ConfigServer.Application.DTOs;
using ConfigServer.Application.Interfaces;
using ConfigServer.Infrastructure.Services;

namespace ConfigServer.Web.Controllers
{  
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigRepository _configRepository;
        private readonly TokenHelper _tokenHelper;
        private readonly IConfigService _configService;
        
        public ConfigController(IConfigRepository configRepository, TokenHelper tokenHelper, IConfigService configService)
        {
            _configRepository = configRepository;
            _tokenHelper = tokenHelper;
            _configService = configService;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Config>>> GetAllConfigs()
        {
            var configs = await _configRepository.GetAllAsync();
            return Ok(configs);
        }

       
        // [HttpGet("{id}")]
        // public async Task<ActionResult<Config>> GetConfigById(Guid id)
        // {
        //     var config = await _configRepository.GetByIdAsync(id);
        //     if (config == null)
        //     {
        //         return NotFound(new { message = "Configuration not found." });
        //     }

        //     return Ok(config);
        //}
        [HttpGet("{Project}")]
        public async Task<ActionResult<Config>> GetConfigByProject(string Project)
        {
            var config = await _configRepository.GetByProjectAsync(Project);
            if (config == null)
            {
                return NotFound(new { message = "Configuration not found." });
            }

            return Ok(config);
        }

        // [HttpPost]
        // public async Task<ActionResult> CreateConfig([FromForm] ConfigDTO configDto, [FromForm] IFormFile configFile)
        // {
        //     if (configFile == null || configFile.Length == 0)
        //     {
        //         return BadRequest(new { message = "No file uploaded." });
        //     }

        //     // Save the file to the server and get the file path
        //     string filePath = await _configFileService.SaveConfigFile(configFile, configDto.ProjectId);

        //     // Create the config entity with the file path
        //     var config = new Config
        //     {
        //         Id = Guid.NewGuid(),
        //         ProjectId = configDto.ProjectId,
        //         FilePath = filePath,
        //         UpdatedAt = DateTime.UtcNow
        //     };

        //     // Save the config entity to the database
        //     await _configRepository.AddAsync(config);
        //     await _configRepository.SaveChangesAsync();

        //     return Ok(config);
        // }

    

    [HttpPost]
    public async Task<ActionResult> CreateConfig([FromForm] ConfigDTO configDto)
    {
        // Input validation
        if (configDto == null || string.IsNullOrWhiteSpace(configDto.Key))
        {
            return BadRequest(new { message = "Key is required." });
        }

        if (string.IsNullOrWhiteSpace(configDto.Value) && 
            (configDto.File == null || configDto.File.Length == 0))
        {
            return BadRequest(new { message = "Provide either Value or a File." });
        }

        try
        {
           
            var (userId, userRole) = _tokenHelper.GetUserFromCookie();

            // Call service to create the config
            var config = await _configService.CreateConfigAsync(configDto, userId, userRole);

            return Ok(config);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"An error occurred: {ex.Message}" });
        }
    }


[HttpPut("{id}")]
public async Task<ActionResult> UpdateConfig(Guid id, [FromForm] UpdateConfigDTO updatedConfig)
{
    var (userId, userRole) = _tokenHelper.GetUserFromCookie();
    if (userRole != "Admin" && userRole != "Editor")
    {
        return Unauthorized(new { message = "You do not have permission to update configurations." });
    }

    try
    {
        await _configService.UpdateConfigAsync(id, updatedConfig);
        return NoContent();
    }
    catch (ArgumentException ex)
    {
        return NotFound(new { message = ex.Message });
    }
    catch (UnauthorizedAccessException ex)
    {
        return Unauthorized(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = $"An error occurred: {ex.Message}" });
    }
}
    }
}