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
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.DependencyInjection;

namespace MongoDb.Atlas.Client.ConsoleApp
{
    static class Program
    {
        #region Inner class

        public class CommandLineOptions
        {
            [Value(0, MetaValue = "Action", Required = true, HelpText = "Action (possible values: \"list\").")]
            public string Action { get; set; }

            [Value(1, MetaValue = "Resource", Required = false, HelpText = "Resource (possible values: \"orgs\").")]
            public string Resource { get; set; }

            [Value(2, MetaValue = "Id", Required = false, HelpText = "ID.")]
            public string Id { get; set; }

            [Option('u', "publickey", Required = false, HelpText = "Public Api key provided by MongoDB Atlas.")]
            public string PublicKey { get; set; }

            [Option('p', "privatekey", Required = false, HelpText = "Private Api key provided by MongoDB Atlas (KEEP IT SECRET!).")]
            public string PrivateKey { get; set; }

            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool IsVerbose { get; set; }
        }

        #endregion

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

            using (var serviceProvider = CreateServiceProvider(opts, configuration))
            {
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
