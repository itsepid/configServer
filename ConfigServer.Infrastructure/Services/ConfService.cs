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

public async Task<Dictionary<string, object>> GetProjectConfigAsync(string projectName, string environment)
{
    
    var project = await _confRepository.GetProjectByNameAsync(projectName);
    if (project == null)
    {
        throw new Exception("Project not found");
    }

    
    var entries = await _confRepository.GetProjectConfigAsync(project.Id, environment);

   
    var configObject = _configHelper.BuildConfig(entries);
    Console.WriteLine($"{project.Id}");

    return configObject;
}



        public async Task CreateProjectConfigAsync(string projectName, Dictionary<string, string> newConfig, string environment)
        {
            var project = await _confRepository.GetProjectByNameAsync(projectName);

            if (project == null) return;

            await _confRepository.UpdateProjectConfigAsync(project.Id, newConfig, environment);

            
        }


        public async Task UpdateProjectConfigAsync(string projectName, Dictionary<string, string> newConfig, string environment)
        {
            var project = await _confRepository.GetProjectByNameAsync(projectName);
            if (project == null)
            {
                throw new Exception("Project not found");
            }

            await _confRepository.UpdateProjectConfigAsync(project.Id, newConfig, environment);

            
            
            Console.WriteLine($"message is published for{projectName}");
        }

        public async Task CreateProjectAsync(string projectName, string passkey)
        {
            var existingProject = await _confRepository.GetProjectByNameAsync(projectName);
            if (existingProject != null) throw new InvalidOperationException("Project already exists.");
            var project = new ConfigProject(projectName, passkey);
            await _confRepository.CreateProjectAsync(project);
            Console.WriteLine($"{project}");
        } 

    }
}
