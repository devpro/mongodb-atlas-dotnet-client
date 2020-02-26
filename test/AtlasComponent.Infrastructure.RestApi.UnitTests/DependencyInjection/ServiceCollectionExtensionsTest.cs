using System.Net.Http;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.DependencyInjection;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.UnitTests.Fakes;
using Xunit;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.UnitTests.DependencyInjection
{
    [Trait("Category", "UnitTests")]
    public class ServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddMongoDbAtlasRestApi_ShouldProvideRepositories()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var configuration = new FakeConfiguration();
            serviceCollection.AddSingleton(new MapperConfiguration(x => { }).CreateMapper());

            // Act
            serviceCollection.AddMongoDbAtlasRestApi(configuration);

            // Assert
            var services = serviceCollection.BuildServiceProvider();
            var organizationRepository = services.GetRequiredService<IOrganizationRepository>();
            organizationRepository.Should().NotBeNull();
            var projectRepository = services.GetRequiredService<IProjectRepository>();
            projectRepository.Should().NotBeNull();
            var ipWhitelistRepository = services.GetRequiredService<IIpWhitelistRepository>();
            ipWhitelistRepository.Should().NotBeNull();
        }

        [Fact]
        public void AddMongoDbAtlasRestApi_ShouldProvideHttpClient()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var configuration = new FakeConfiguration();
            serviceCollection.AddSingleton(new MapperConfiguration(x => { }).CreateMapper());

            // Act
            serviceCollection.AddMongoDbAtlasRestApi(configuration);

            // Assert
            var services = serviceCollection.BuildServiceProvider();
            var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull();
            var client = httpClientFactory.CreateClient(configuration.HttpClientName);
            client.Should().NotBeNull();
        }
    }
}
