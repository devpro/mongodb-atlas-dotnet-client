using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.DependencyInjection
{
    /// <summary>
    /// Service collection. extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the dependency injection configuration.
        /// </summary>
        /// <typeparam name="T">Instance of <see cref="IMongoDbAtlasRestApiConfiguration"/></typeparam>
        /// <param name="services">Collection of services that will be completed</param>
        /// <returns></returns>
        public static IServiceCollection AddMongoDbAtlasRestApi<T>(this IServiceCollection services, T configuration)
            where T : class, IMongoDbAtlasRestApiConfiguration
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<IMongoDbAtlasRestApiConfiguration, T>();
            services.TryAddTransient<Domain.Repositories.IOrganizationRepository, Repositories.OrganizationRepository>();
            services.TryAddTransient<Domain.Repositories.IProjectRepository, Repositories.ProjectRepository>();
            services.TryAddTransient<Domain.Repositories.IIpWhitelistRepository, Repositories.IpWhitelistRepository>();

            // hack: needed to make the Digest call work
            // see: https://stackoverflow.com/questions/53764083/use-http-2-with-httpclient-in-net,
            //  https://docs.microsoft.com/en-us/dotnet/api/system.net.http.socketshttphandler?view=netcore-3.1,
            //  https://github.com/dotnet/corefx/issues/37729
            AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);

            services
                .AddHttpClient(configuration.HttpClientName)
                .ConfigurePrimaryHttpMessageHandler(
                    x => new HttpClientHandler
                    {
                        Credentials = new NetworkCredential(configuration.PublicKey, configuration.PrivateKey)
                    });

            return services;
        }
    }
}
