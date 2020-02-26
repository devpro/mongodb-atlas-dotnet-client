using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.DependencyInjection;
using MongoDb.Atlas.Client.ConsoleApp.Tasks;
using MongoDb.Atlas.Client.ConsoleApp.UnitTests.Fakes;
using Xunit;

namespace MongoDb.Atlas.Client.ConsoleApp.UnitTests.Tasks
{
    [Trait("Category", "UnitTests")]
    public class ConsoleTaskFactoryTest
    {
        protected readonly ServiceProvider _serviceProvider;

        public ConsoleTaskFactoryTest()
        {
            var services = new ServiceCollection()
                .AddLogging()
                .AddMongoDbAtlasRestApi(new AppConfigurationFake())
                .AddSingleton(new MapperConfiguration(x => { }).CreateMapper());

            _serviceProvider = services.BuildServiceProvider();
        }

        [Theory]
        [InlineData("list", "orgs", "ListOrganizationTask")]
        [InlineData("list", "projects", "ListProjectTask")]
        [InlineData("list", "events", "ListEventTask")]
        [InlineData("list", "whitelist", "ListIpWhitelistTask")]
        [InlineData("edit", "whitelist", "EditIpWhitelistTask")]
        [InlineData("delete", "whitelist", "DeleteIpWhitelistTask")]
        public void ConsoleTaskFactoryCreate_ShouldCoverAllOptions(string action, string resource, string taskClassName)
        {
            // Arrange
            var factory = new ConsoleTaskFactory(_serviceProvider);

            // Act
            var task = factory.Create(action, resource, out var errorMessage);

            // Assert
            errorMessage.Should().BeNull();
            task.Should().NotBeNull();
            task.GetType().Name.Should().Be(taskClassName);
        }

        [Theory]
        [InlineData("list", null, "Unknown resource \"\". Available resources: \"events\", \"orgs\", \"projects\", \"whitelist\"")]
        [InlineData("list", "", "Unknown resource \"\". Available resources: \"events\", \"orgs\", \"projects\", \"whitelist\"")]
        [InlineData("edit", "orgs", "Unknown action \"edit\" for resource \"orgs\". Possible actions: \"list\"")]
        [InlineData("create", "projects", "Unknown action \"create\" for resource \"projects\". Possible actions: \"list\"")]
        [InlineData("tail", "events", "Unknown action \"tail\" for resource \"events\". Possible actions: \"list\"")]
        [InlineData("show", "whitelist", "Unknown action \"show\" for resource \"whitelist\". Possible actions: \"list\", \"create\", \"delete\"")]
        public void ConsoleTaskFactoryCreate_ShouldSetErrorMessageOnInvalidOptions(string action, string resource, string expected)
        {
            // Arrange
            var factory = new ConsoleTaskFactory(_serviceProvider);

            // Act
            var task = factory.Create(action, resource, out var actual);

            // Assert
            task.Should().BeNull();
            actual.Should().NotBeNull();
            actual.Should().Be(expected);
        }
    }
}
