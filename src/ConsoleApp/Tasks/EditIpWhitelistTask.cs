using System.Collections.Generic;
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
            if (string.IsNullOrEmpty(options.Project) || string.IsNullOrEmpty(options.Values))
            {
                return null;
            }

            _logger.LogDebug("Retrieve ip whitelist for project");

            var whitelist = await _ipWhitelistRepository.FindAllAsync(options.Project);

            var input = options.Values.Split(",");

            var newEntries = new List<IpWhitelistRecordModel>();
            foreach (var entry in input)
            {
                var ipAddress = entry.Split(":")[0];
                var comment = entry.Contains(':') ? entry.Split(":")[1] : "Created by mdbatlas";
                var cidr = $"{ipAddress}/32";
                if (!whitelist.Any(x => x.CidrBlock == cidr || x.IpAddress == ipAddress))
                {
                    newEntries.Add(new IpWhitelistRecordModel { CidrBlock = cidr, Comment = comment });
                }
            }

            if (newEntries.Any())
            {
                _logger.LogDebug($"Add {newEntries.Count} new ip whitelist entrie(s)");

                _ = await _ipWhitelistRepository.CreateAsync(options.Project, newEntries);
            }

            return $"IP white list updated ({newEntries.Count} added on initial {whitelist.Count})";
        }
    }
}
