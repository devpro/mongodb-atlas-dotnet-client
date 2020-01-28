using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.DependencyInjection;

namespace MongoDb.Atlas.Client.ConsoleApp
{
    static class Program
    {
        #region Inner class

        public class CommandLineOptions
        {
            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool IsVerbose { get; set; }

            [Option('a', "action", Required = true, HelpText = "Action (possible values: \"list\").")]
            public string Action { get; set; }

            [Option('r', "resource", Required = true, HelpText = "Resource (possible values: \"orgs\").")]
            public string Resource { get; set; }

            [Option("id", Required = false, HelpText = "ID.")]
            public string Id { get; set; }
        }

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

        private async static Task<int> RunOptionsAndReturnExitCode(CommandLineOptions opts)
        {
            var configuration = CreateConfiguration();
            using (var serviceProvider = CreateServiceProvider(opts, configuration))
            {
                switch (opts.Action)
                {
                    case "list":
                        if (opts.Resource == "orgs")
                        {
                            LogVerbose(opts, "Query the organizations collection");

                            var organizationRepository = serviceProvider.GetService<IOrganizationRepository>();
                            var orgs = await organizationRepository.FindAll();

                            Console.WriteLine($"Items found: {orgs.Count}");
                            Console.WriteLine($"First organization found: {orgs.FirstOrDefault().Name}");
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
        }

        private static int HandleParseError()
        {
            return -2;
        }

        #endregion

        #region Private helpers

        private static IConfigurationRoot CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location))
                .AddJsonFile($"appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }

        private static ServiceProvider CreateServiceProvider(CommandLineOptions opts, IConfigurationRoot configuration)
        {
            LogVerbose(opts, "Create the service provider");
            var serviceCollection = new ServiceCollection()
                .AddLogging(builder => { builder.AddConsole(); })
                .AddSingleton(configuration)
                .AddMongoDbAtlasRestApi<AppConfiguration>(new AppConfiguration(configuration));

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
