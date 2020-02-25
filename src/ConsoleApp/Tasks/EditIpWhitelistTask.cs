using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Models;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;

namespace MongoDb.Atlas.Client.ConsoleApp.Tasks
{
    public class EditIpWhitelistTask : IConsoleTask
    {
        private readonly ILogger<EditIpWhitelistTask> _logger;
        private readonly IIpWhitelistRepository _ipWhitelistRepository;

        public EditIpWhitelistTask(ILogger<EditIpWhitelistTask> logger, IIpWhitelistRepository ipWhitelistRepository)
        {
            _logger = logger;
            _ipWhitelistRepository = ipWhitelistRepository;
        }

        public async Task<string> ExecuteAsync(CommandLineOptions options)
        {
            if (string.IsNullOrEmpty(options.Project) || string.IsNullOrEmpty(options.Project))
            {
                return null;
            }

            _logger.LogDebug("Retrieve ip whitelist for project");

            var whitelist = await _ipWhitelistRepository.FindAllAsync(options.Project);

            var input = options.Values.Split(",");

            var nbCreated = 0;
            foreach (var entry in input)
            {
                var ipAddress = entry.Split(":")[0];
                var comment = entry.Contains(':') ? entry.Split(":")[1] : "Created by mdbatlas";
                var cidr = $"{ipAddress}/32";
                if (!whitelist.Any(x => x.CidrBlock == cidr || x.IpAddress == ipAddress))
                {
                    _logger.LogDebug("Add new ip whitelist");

                    _ = await _ipWhitelistRepository.CreateAsync(options.Project, cidr, comment);
                    nbCreated++;
                }
            }

            return $"IP white list updated ({nbCreated} added on initial {whitelist.Count})";
        }
    }
}
