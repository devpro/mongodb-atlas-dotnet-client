using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;

namespace MongoDb.Atlas.Client.ConsoleApp.Tasks
{
    public class ConsoleTaskFactory
    {
        private readonly ServiceProvider _serviceProvider;

        public ConsoleTaskFactory(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IConsoleTask Create(string action, string resource, out string errorMessage)
        {
            errorMessage = null;
            switch (resource)
            {
                case "events":
                    if (action == "list")
                    {
                        return new ListEventTask(_serviceProvider.GetService<ILogger<ListEventTask>>(),
                            _serviceProvider.GetService<IProjectRepository>());
                    }
                    else
                    {
                        errorMessage = $"Unknown action \"{action}\" for resource \"{resource}\". Possible actions: \"list\"";
                        return null;
                    }
                case "orgs":
                    if (action == "list")
                    {
                        return new ListOrganizationTask(_serviceProvider.GetService<ILogger<ListOrganizationTask>>(),
                            _serviceProvider.GetService<IOrganizationRepository>());
                    }
                    else
                    {
                        errorMessage = $"Unknown action \"{action}\" for resource \"{resource}\". Possible actions: \"list\"";
                        return null;
                    }
                case "projects":
                    if (action == "list")
                    {
                        return new ListProjectTask(_serviceProvider.GetService<ILogger<ListProjectTask>>(),
                            _serviceProvider.GetService<IProjectRepository>());
                    }
                    else
                    {
                        errorMessage = $"Unknown action \"{action}\" for resource \"{resource}\". Possible actions: \"list\"";
                        return null;
                    }
                case "whitelist":
                    if (action == "list")
                    {
                        return new ListIpWhitelistTask(_serviceProvider.GetService<ILogger<ListIpWhitelistTask>>(),
                            _serviceProvider.GetService<IIpWhitelistRepository>());
                    }
                    else if (action == "edit")
                    {
                        return new EditIpWhitelistTask(_serviceProvider.GetService<ILogger<EditIpWhitelistTask>>(),
                            _serviceProvider.GetService<IIpWhitelistRepository>());
                    }
                    else if (action == "delete")
                    {
                        return new DeleteIpWhitelistTask(_serviceProvider.GetService<ILogger<DeleteIpWhitelistTask>>(),
                            _serviceProvider.GetService<IIpWhitelistRepository>());
                    }
                    else
                    {
                        errorMessage = $"Unknown action \"{action}\" for resource \"{resource}\". Possible actions: \"list\", \"create\", \"delete\"";
                        return null;
                    }
                default:
                    errorMessage = $"Unknown resource \"{resource}\". Available resources: \"events\", \"orgs\", \"projects\", \"whitelist\"";
                    return null;
            }
        }
    }
}
