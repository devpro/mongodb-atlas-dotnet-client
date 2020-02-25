using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Models;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;

namespace MongoDb.Atlas.Client.ConsoleApp.Tasks
{
    public class DeleteIpWhitelistTask : IConsoleTask
    {
        private readonly ILogger<DeleteIpWhitelistTask> _logger;
        private readonly IIpWhitelistRepository _ipWhitelistRepository;

        public DeleteIpWhitelistTask(ILogger<DeleteIpWhitelistTask> logger, IIpWhitelistRepository ipWhitelistRepository)
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

            var existingEntries = new List<IpWhitelistRecordModel>();
            foreach (var entry in input)
            {
                var ipAddress = entry.Split(":")[0];
                var cidr = $"{ipAddress}/32";
                if (whitelist.Any(x => x.CidrBlock == cidr || x.IpAddress == ipAddress))
                {
                    existingEntries.Add(new IpWhitelistRecordModel { CidrBlock = cidr });
                }
            }

            if (existingEntries.Any())
            {
                _logger.LogDebug($"Remove {existingEntries.Count} entrie(s) from the ip whitelist entrie(s)");

                foreach (var existing in existingEntries)
                {
                    await _ipWhitelistRepository.DeleteAsync(options.Project, existing);
                }
            }

            return $"IP white list updated ({existingEntries.Count} removed on initial {whitelist.Count})";
        }
    }
}
