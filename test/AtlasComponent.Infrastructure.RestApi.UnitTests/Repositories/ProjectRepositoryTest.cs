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
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.UnitTests.Fakes;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.UnitTests.Repositories
{
    [Trait("Category", "UnitTests")]
    public class ProjectRepositoryTest : RepositoryTestBase
    {
        #region Private fields & Constructor

        public ProjectRepositoryTest()
            : base()
        {
        }

        #endregion

        #region FindAll test methods

        [Fact]
        public async Task ProjectRepositoryFindAll_ReturnListFromApiCall()
        {
            // Arrange
            var fixture = new Fixture();
            var responseDto = fixture.Create<ResultListDto<ProjectDto>>();
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(responseDto))
            };
            var repository = BuildRepository(httpResponseMessage);

            // Act
            var output = await repository.FindAllAsync();

            // Assert
            output.Should().NotBeNull();
            output.Should().HaveCount(responseDto.Results.Count);
            output.First().Should().BeEquivalentTo(Mapper.Map<ProjectModel>(responseDto.Results.First()));
        }

        #endregion

        #region Private methods

        private IProjectRepository BuildRepository(HttpResponseMessage httpResponseMessage)
        {
            var configuration = new DummyMongoDbAtlasRestApiConfiguration();

            var logger = ServiceProvider.GetService<ILogger<ProjectRepository>>();

            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>()))
                .Returns(httpResponseMessage);

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(configuration.HttpClientName))
                .Returns(httpClient);

            return new ProjectRepository(configuration, logger, httpClientFactoryMock.Object, Mapper);
        }

        #endregion
    }
}
