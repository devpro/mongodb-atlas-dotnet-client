using System;
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
        public static IServiceCollection AddMongoDbAtlasRestApi<T>(this IServiceCollection services)
            where T : class, IMongoDbAtlasRestApiConfiguration
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<IMongoDbAtlasRestApiConfiguration, T>();
            services.TryAddTransient<Domain.Repositories.IOrganizationRepository, Repositories.OrganizationRepository>();

            return services;
        }
    }
}
