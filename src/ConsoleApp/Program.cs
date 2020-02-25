using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Models;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.DependencyInjection;
using Withywoods.Serialization.Json;
using Withywoods.System;

namespace MongoDb.Atlas.Client.ConsoleApp
{
    static class Program
    {
        #region Constants

        private const string _AppsettingsFilename = "appsettings.json";

        #endregion

        #region Entry point

        private async static Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(
                    (CommandLineOptions opts) => RunOptionsAndReturnExitCode(opts),
                    errs => Task.FromResult(HandleParseError())
                 );
        }

        #endregion

        #region Private helpers

        private async static Task<int> RunOptionsAndReturnExitCode(CommandLineOptions opts)
        {
            if (opts.Action == "config")
            {
                if (string.IsNullOrEmpty(opts.PublicKey) || string.IsNullOrEmpty(opts.PrivateKey))
                {
                    Console.WriteLine("Invalid arguments. Both public and private keys must be set.");
                    return -1;
                }
                SaveSettings(opts);
                return 0;
            }

            var configuration = LoadConfiguration();
            var appConfiguration = new AppConfiguration(configuration);
            if (string.IsNullOrEmpty(((IMongoDbAtlasRestApiConfiguration)appConfiguration).PublicKey)
                || string.IsNullOrEmpty(((IMongoDbAtlasRestApiConfiguration)appConfiguration).PrivateKey))
            {
                Console.WriteLine("Missing configuration. Please use the config command or set the keys manually.");
                return -1;
            }

            using var serviceProvider = CreateServiceProvider(opts, configuration);

            switch (opts.Action)
            {
                case "list":
                    if (string.IsNullOrEmpty(opts.Resource))
                    {
                        Console.WriteLine("The resource must be specified");
                        return -1;
                    }

                    if (opts.Resource == "orgs")
                    {
                        LogVerbose(opts, "Query the organizations collection");

                        var organizationRepository = serviceProvider.GetService<IOrganizationRepository>();
                        var orgs = await organizationRepository.FindAllAsync();

                        if (!string.IsNullOrEmpty(opts.Query))
                        {
                            var property = typeof(OrganizationModel).GetProperty(opts.Query.FirstCharToUpper());
                            Console.WriteLine(property.GetValue(orgs.First()));
                        }
                        else
                        {
                            Console.WriteLine($"Items found: {orgs.Count}");
                            Console.WriteLine($"First organization found: {orgs.FirstOrDefault().Name}");
                        }
                    }
                    else if (opts.Resource == "projects")
                    {
                        LogVerbose(opts, "Query the projects collection");

                        var projectRepository = serviceProvider.GetService<IProjectRepository>();
                        var projects = await projectRepository.FindAllAsync();

                        if (!string.IsNullOrEmpty(opts.Query))
                        {
                            var property = typeof(ProjectModel).GetProperty(opts.Query.FirstCharToUpper());
                            Console.WriteLine(property.GetValue(projects.First()));
                        }
                        else
                        {
                            Console.WriteLine($"Items found: {projects.Count}");
                            Console.WriteLine($"First organization found: {projects.FirstOrDefault().Name}");
                        }
                    }
                    else if (opts.Resource == "events")
                    {
                        if (string.IsNullOrEmpty(opts.Project))
                        {
                            Console.WriteLine("The project must be specified");
                            return -1;
                        }

                        LogVerbose(opts, "Query the projects collection");

                        var projectRepository = serviceProvider.GetService<IProjectRepository>();
                        var events = await projectRepository.FindAllEventsByProjectIdAsync(opts.Project);

                        Console.WriteLine(events.ToJson());
                    }
                    else if (opts.Resource == "whitelist")
                    {
                        if (string.IsNullOrEmpty(opts.Project))
                        {
                            Console.WriteLine("The project must be specified");
                            return -1;
                        }

                        LogVerbose(opts, "Query the projects collection");

                        var whitelistRepository = serviceProvider.GetService<IIpWhitelistRepository>();
                        var whitelist = await whitelistRepository.FindAllAsync(opts.Project);

                        Console.WriteLine(whitelist.ToJson());
                    }
                    else
                    {
                        Console.WriteLine($"Unknown resource \"{opts.Resource}\"");
                        return -1;
                    }
                    break;
                default:
                    Console.WriteLine($"Unknown action \"{opts.Action}\"");
                    return -1;
            }

            return 0;
        }

        private static int HandleParseError()
        {
            return -2;
        }

        private static IConfigurationRoot LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location))
                .AddJsonFile(_AppsettingsFilename, true, true)
                .AddEnvironmentVariables()
                .Build();
        }

        private static void SaveSettings(CommandLineOptions opts)
        {
            var jsonString = $"{{\"mdbatlas\":{{\"PublicKey\": \"{opts.PublicKey}\", \"PrivateKey\": \"{opts.PrivateKey}\"}}}}";
            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, _AppsettingsFilename), jsonString, Encoding.UTF8);
        }

        private static ServiceProvider CreateServiceProvider(CommandLineOptions opts, IConfigurationRoot configuration)
        {
            LogVerbose(opts, "Create the service provider");
            var serviceCollection = new ServiceCollection()
                .AddLogging(builder =>
                    {
                        builder
                            .AddFilter("Microsoft", opts.IsVerbose ? LogLevel.Information : LogLevel.Warning)
                            .AddFilter("System", opts.IsVerbose ? LogLevel.Information : LogLevel.Warning)
                            .AddFilter("MongoDb.Atlas.Client", opts.IsVerbose ? LogLevel.Debug : LogLevel.Information)
                            .AddConsole();
                    })
                .AddSingleton(configuration)
                .AddMongoDbAtlasRestApi(new AppConfiguration(configuration));

            ConfigureAutoMapper(serviceCollection);

            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureAutoMapper(IServiceCollection serviceCollection)
        {
            var mappingConfig = new MapperConfiguration(x =>
            {
                x.AddProfile(new AtlasComponent.Infrastructure.RestApi.MappingProfiles.GenericMappingProfile());
                x.AllowNullCollections = true;
            });
            var mapper = mappingConfig.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            serviceCollection.AddSingleton(mapper);
        }

        private static void LogVerbose(CommandLineOptions opts, string message)
        {
            if (opts.IsVerbose)
            {
                Console.WriteLine(message);
            }
        }

        #endregion
    }
}
