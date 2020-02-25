using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Models;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;
using Withywoods.System;

namespace MongoDb.Atlas.Client.ConsoleApp.Tasks
{
    public class ListOrganizationTask : IConsoleTask
    {
        private readonly ILogger<ListOrganizationTask> _logger;
        private readonly IOrganizationRepository _organizationRepository;

        public ListOrganizationTask(ILogger<ListOrganizationTask> logger, IOrganizationRepository organizationRepository)
        {
            _logger = logger;
            _organizationRepository = organizationRepository;
        }

        public async Task<string> ExecuteAsync(CommandLineOptions options)
        {
            _logger.LogDebug("Query the organizations collection");

            var orgs = await _organizationRepository.FindAllAsync();
            if (!orgs.Any())
            {
                return null;
            }

            if (!string.IsNullOrEmpty(options.Query))
            {
                var property = typeof(OrganizationModel).GetProperty(options.Query.FirstCharToUpper());
                return property.GetValue(orgs.First()).ToString();
            }

            return $"Items found: {orgs.Count}. First organization found: {orgs.FirstOrDefault().Name}";
        }
    }
}
