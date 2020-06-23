using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.DependencyInjection;
using MongoDb.Atlas.Client.ConsoleApp.Tasks;

namespace MongoDb.Atlas.Client.ConsoleApp
{
    static class Program
    {
        private const string AppsettingsFilename = "appsettings.json";

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

            var factory = new ConsoleTaskFactory(serviceProvider);

            var task = factory.Create(opts.Action, opts.Resource, out var errorMessage);
            if (task == null)
            {
                Console.WriteLine(errorMessage);
                return -1;
            }

            try
            {
                var output = await task.ExecuteAsync(opts);
                if (string.IsNullOrEmpty(output))
                {
                    Console.WriteLine("No data returned");
                    return -1;
                }

                Console.WriteLine(output);
                return 0;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"An error occured: {exc.Message}");
                return -2;
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
                .AddJsonFile(AppsettingsFilename, true, true)
                .AddEnvironmentVariables()
                .Build();
        }

        private static void SaveSettings(CommandLineOptions opts)
        {
            var jsonString = $"{{\"mdbatlas\":{{\"PublicKey\": \"{opts.PublicKey}\", \"PrivateKey\": \"{opts.PrivateKey}\"}}}}";
            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, AppsettingsFilename), jsonString, Encoding.UTF8);
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
    }
}
