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
using Newtonsoft.Json;
using Xunit;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.UnitTests.Repositories
{
    [Trait("Category", "UnitTests")]
    public class IpWhitelistRepositoryTest : RepositoryTestBase
    {
        #region Private fields & Constructor

        public IpWhitelistRepositoryTest()
            : base()
        {
        }

        #endregion

        #region FindAll test methods

        [Fact]
        public async Task IpWhitelistRepositoryRepositoryFindAll_ReturnListFromApiCall()
        {
            // Arrange
            var fixture = new Fixture();
            var responseDto = fixture.Create<ResultListDto<IpWhitelistRecordDto>>();
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(responseDto))
            };
            var repository = BuildRepository(httpResponseMessage, HttpMethod.Get, "https://dummy.mongodb.com/api/atlas/v1.0/groups/42/whitelist");

            // Act
            var output = await repository.FindAllAsync("42");

            // Assert
            output.Should().NotBeNull();
            output.Should().HaveCount(responseDto.Results.Count);
            output.First().Should().BeEquivalentTo(Mapper.Map<IpWhitelistRecordModel>(responseDto.Results.First()));
        }

        #endregion

        #region Private methods

        private IIpWhitelistRepository BuildRepository(HttpResponseMessage httpResponseMessage, HttpMethod httpMethod, string absoluteUri)
        {
            var logger = ServiceProvider.GetService<ILogger<IpWhitelistRepository>>();

            var httpClientFactoryMock = BuildHttpClientFactory(httpResponseMessage, httpMethod, absoluteUri);

            return new IpWhitelistRepository(Configuration, logger, httpClientFactoryMock.Object, Mapper);
        }

        #endregion
    }
}
