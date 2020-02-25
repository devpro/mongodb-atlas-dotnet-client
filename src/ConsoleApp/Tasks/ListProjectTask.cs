
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Models;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;
using Withywoods.System;

namespace MongoDb.Atlas.Client.ConsoleApp.Tasks
{
    public class ListProjectTask : IConsoleTask
    {
        private readonly ILogger<ListProjectTask> _logger;
        private readonly IProjectRepository _projectRepository;

        public ListProjectTask(ILogger<ListProjectTask> logger, IProjectRepository projectRepository)
        {
            _logger = logger;
            _projectRepository = projectRepository;
        }

        public async Task<string> ExecuteAsync(CommandLineOptions options)
        {
            _logger.LogDebug("Query the projects collection");

            var projects = await _projectRepository.FindAllAsync();
            if (!projects.Any())
            {
                return null;
            }

            if (!string.IsNullOrEmpty(options.Query))
            {
                var property = typeof(ProjectModel).GetProperty(options.Query.FirstCharToUpper());
                return property.GetValue(projects.First()).ToString();
            }

            return $"Items found: {projects.Count}. First project found: {projects.FirstOrDefault().Name}";
        }
    }
}
