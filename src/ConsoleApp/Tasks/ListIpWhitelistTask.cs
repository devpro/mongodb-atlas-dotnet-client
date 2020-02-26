using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;
using Withywoods.Serialization.Json;

namespace MongoDb.Atlas.Client.ConsoleApp.Tasks
{
    public class ListIpWhitelistTask : IConsoleTask
    {
        private readonly ILogger _logger;
        private readonly IIpWhitelistRepository _ipWhitelistRepository;

        public ListIpWhitelistTask(ILogger<ListIpWhitelistTask> logger, IIpWhitelistRepository ipWhitelistRepository)
        {
            _logger = logger;
            _ipWhitelistRepository = ipWhitelistRepository;
        }

        public async Task<string> ExecuteAsync(CommandLineOptions options)
        {
            if (string.IsNullOrEmpty(options.Project))
            {
                return null;
            }

            _logger.LogDebug("Query the projects collection");

            var whitelist = await _ipWhitelistRepository.FindAllAsync(options.Project);

            return whitelist.ToJson();
        }
    }
}
