using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;
using Withywoods.Serialization.Json;

namespace MongoDb.Atlas.Client.ConsoleApp.Tasks
{
    class ListEventTask : IConsoleTask
    {
        private readonly ILogger<ListEventTask> _logger;
        private readonly IProjectRepository _projectRepository;

        public ListEventTask(ILogger<ListEventTask> logger, IProjectRepository projectRepository)
        {
            _logger = logger;
            _projectRepository = projectRepository;
        }

        public async Task<string> ExecuteAsync(CommandLineOptions options)
        {
            if (string.IsNullOrEmpty(options.Project))
            {
                return null;
            }

            _logger.LogDebug("Query the events collection");

            var events = await _projectRepository.FindAllEventsByProjectIdAsync(options.Project);
            if (!events.Any())
            {
                return null;
            }

            return events.ToJson();
        }
    }
}
