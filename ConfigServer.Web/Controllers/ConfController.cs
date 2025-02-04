using ConfigServer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ConfigServer.Infrastructure.Services;
using System.Threading.Tasks;

namespace ConfigServer.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfController : ControllerBase
    {
        private readonly IConfService _configService;
        private readonly ConfigHelper _configHelper;

        public ConfController(IConfService configService, ConfigHelper configHelper)
        {
            _configService = configService;
            _configHelper = configHelper;
        }

        // Create project
        [HttpPost("create-project")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            try
            {
                await _configService.CreateProjectAsync(request.ProjectName, request.Passkey, request.Environment);
                return Ok(new { Message = "Project created successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Get project config
     [HttpGet("get-config/{projectName}")]
public async Task<IActionResult> GetProjectConfig(string projectName, [FromHeader] string passkey)
{
    // Validate the passkey.
    bool isValid = await _configService.ValidatePasskeyAsync(projectName, passkey);
    if (!isValid)
    {
        return Unauthorized("Invalid passkey.");
    }

    // Get the nested configuration object.
    var config = await _configService.GetProjectConfigAsync(projectName);

    // Let ASP.NET Core handle serialization by returning the object directly.
    return Ok(config);
}


        // Update project config
        [HttpPost("update-config/{projectName}")]
        public async Task<IActionResult> UpdateProjectConfig(string projectName, [FromBody] Dictionary<string, string> newConfig, [FromHeader] string passkey)
        {
            var isValidPasskey = await _configService.ValidatePasskeyAsync(projectName, passkey);

            if (!isValidPasskey)
            {
                return Unauthorized("Invalid passkey.");
            }

            await _configService.CreateProjectConfigAsync(projectName, newConfig);

            return Ok(new { Message = "Configuration updated successfully." });
        }


            [HttpPut("{projectName}/configs")]
        public async Task<IActionResult> UpdateProjectConfig(string projectName, [FromBody] Dictionary<string, string> updatedConfigs)
        {
            try
            {
                await _configService.UpdateProjectConfigAsync(projectName, updatedConfigs);
                return Ok("Configurations updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


}


        
    }

    public class CreateProjectRequest
    {
        public string ProjectName { get; set; }
        public string Passkey { get; set; }

        public string Environment { get; set; }
    }

    public class ConfigUpdateRequest
{
    public string Key { get; set; }
    public string Value { get; set; }
}

