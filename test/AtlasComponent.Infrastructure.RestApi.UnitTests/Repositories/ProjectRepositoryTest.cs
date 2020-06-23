using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Models;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Dto;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Repositories;
using Withywoods.Serialization.Json;
using Xunit;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.UnitTests.Repositories
{
    [Trait("Category", "UnitTests")]
    public class ProjectRepositoryTest : RepositoryTestBase
    {
        public ProjectRepositoryTest()
            : base()
        {
        }

        [Fact]
        public async Task ProjectRepositoryFindOneByNameAsync_WithExistingName_ReturnRelatedProject()
        {
            // Arrange
            var fixture = new Fixture();
            var responseDto = fixture.Create<ProjectDto>();
            var name = responseDto.Name;
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseDto.ToJson())
            };
            var repository = BuildRepository(httpResponseMessage, HttpMethod.Get, $"https://dummy.mongodb.com/api/atlas/v1.0/groups/byName/{name}");

            // Act
            var output = await repository.FindOneByNameAsync(name);

            // Assert
            output.Should().NotBeNull();
            output.Should().BeEquivalentTo(Mapper.Map<ProjectModel>(responseDto));
        }

        [Fact]
        public async Task ProjectRepositoryFindAll_ReturnListFromApiCall()
        {
            // Arrange
            var fixture = new Fixture();
            var responseDto = fixture.Create<ResultListDto<ProjectDto>>();
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseDto.ToJson())
            };
            var repository = BuildRepository(httpResponseMessage, HttpMethod.Get, "https://dummy.mongodb.com/api/atlas/v1.0/groups");

            // Act
            var output = await repository.FindAllAsync();

            // Assert
            output.Should().NotBeNull();
            output.Should().HaveCount(responseDto.Results.Count);
            output.First().Should().BeEquivalentTo(Mapper.Map<ProjectModel>(responseDto.Results.First()));
        }

        [Fact]
        public async Task ProjectRepositoryFindAllEventsByProjectId_ReturnListFromApiCall()
        {
            // Arrange
            var fixture = new Fixture();
            var responseDto = fixture.Create<ResultListDto<EventDto>>();
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseDto.ToJson())
            };
            var repository = BuildRepository(httpResponseMessage, HttpMethod.Get, "https://dummy.mongodb.com/api/atlas/v1.0/groups/42/events");

            // Act
            var output = await repository.FindAllEventsByProjectIdAsync("42");

            // Assert
            output.Should().NotBeNull();
            output.Should().HaveCount(responseDto.Results.Count);
            output.First().Should().BeEquivalentTo(Mapper.Map<EventModel>(responseDto.Results.First()));
        }

        private IProjectRepository BuildRepository(HttpResponseMessage httpResponseMessage, HttpMethod httpMethod, string absoluteUri)
        {
            var logger = ServiceProvider.GetService<ILogger<ProjectRepository>>();

            var httpClientFactoryMock = BuildHttpClientFactory(httpResponseMessage, httpMethod, absoluteUri);

            return new ProjectRepository(Configuration, logger, httpClientFactoryMock.Object, Mapper);
        }
    }
}
