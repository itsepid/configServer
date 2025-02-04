using ConfigServer.Application.Interfaces;
using ConfigServer.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigServer.Infrastructure.Services
{
    public class ConfService : IConfService
    {
        private readonly IConfRepository _confRepository;
        private readonly IRabbitMQService _rabbitMQService;

        private readonly ConfigHelper _configHelper;

        public ConfService(IConfRepository confRepository, ConfigHelper configHelper, IRabbitMQService rabbitMQService)
        {
            _confRepository = confRepository;
            _configHelper = configHelper;
            _rabbitMQService = rabbitMQService;
        }

        public async Task<bool> ValidatePasskeyAsync(string projectName, string passkey)
        {
            var project = await _confRepository.GetProjectByNameAsync(projectName);

            if (project == null) return false;

            return project.VerifyPasskey(passkey);
        }

        // public async Task<IEnumerable<ConfigEntry>> GetProjectConfigAsync(string projectName)
        // {
        //     var project = await _confRepository.GetProjectByNameAsync(projectName);

        //     if (project == null) return null;

        //     return await _confRepository.GetProjectConfigAsync(project.Id);
        // }

public async Task<Dictionary<string, object>> GetProjectConfigAsync(string projectName)
{
    // Retrieve the project based on the project name.
    var project = await _confRepository.GetProjectByNameAsync(projectName);
    if (project == null)
    {
        throw new Exception("Project not found");
    }

    // Retrieve all configuration entries for the project.
    var entries = await _confRepository.GetProjectConfigAsync(project.Id);

    // Use the ConfigHelper to build the nested configuration object.
    var configObject = _configHelper.BuildConfig(entries);

    return configObject;
}



        public async Task CreateProjectConfigAsync(string projectName, Dictionary<string, string> newConfig)
        {
            var project = await _confRepository.GetProjectByNameAsync(projectName);

            if (project == null) return;

            await _confRepository.UpdateProjectConfigAsync(project.Id, newConfig);

            
        }


        public async Task UpdateProjectConfigAsync(string projectName, Dictionary<string, string> newConfig)
        {
            var project = await _confRepository.GetProjectByNameAsync(projectName);
            if (project == null)
            {
                throw new Exception("Project not found");
            }

            await _confRepository.UpdateProjectConfigAsync(project.Id, newConfig);

            // Optionally, notify other systems about the update
            await _rabbitMQService.PublishConfigUpdateAsync(projectName, newConfig);
            Console.WriteLine($"message is published for{projectName}");
        }

        public async Task CreateProjectAsync(string projectName, string passkey, string environment)
        {
            var existingProject = await _confRepository.GetProjectByNameAsync(projectName);
            if (existingProject != null) throw new InvalidOperationException("Project already exists.");
            var project = new ConfigProject(projectName, passkey, environment);
            await _confRepository.CreateProjectAsync(project);
        } 

    }
}
