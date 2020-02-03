using System.Net.Http;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Repositories;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.UnitTests.Fakes;
using Moq;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.UnitTests.Repositories
{
    public abstract class RepositoryTestBase
    {
        #region Protected properties & Constructor

        protected ServiceProvider ServiceProvider { get; private set; }
        protected IMapper Mapper { get; private set; }
        protected IMongoDbAtlasRestApiConfiguration Configuration { get; set; }
        protected Mock<FakeHttpMessageHandler> HttpMessageHandlerMock { get; set; }

        protected RepositoryTestBase()
        {
            Mapper = BuildAutoMapper();
            var services = new ServiceCollection()
                .AddLogging();
            ServiceProvider = services.BuildServiceProvider();
            Configuration = new DummyMongoDbAtlasRestApiConfiguration();
            HttpMessageHandlerMock = new Mock<FakeHttpMessageHandler> { CallBase = true };
        }

        #endregion

        #region Protected methods

        protected virtual IMapper BuildAutoMapper()
        {
            var mappingConfig = new MapperConfiguration(x =>
            {
                x.AddProfile(new RestApi.MappingProfiles.GenericMappingProfile());
                x.AllowNullCollections = true;
            });
            return mappingConfig.CreateMapper();
        }

        protected virtual Mock<IHttpClientFactory> BuildHttpClientFactory(HttpResponseMessage httpResponseMessage, HttpMethod httpMethod, string absoluteUri)
        {
            HttpMessageHandlerMock.Setup(f => f.Send(It.Is<HttpRequestMessage>(m =>
                    m.Method == httpMethod
                    && m.RequestUri.AbsoluteUri == absoluteUri)))
                .Returns(httpResponseMessage);

            var httpClient = new HttpClient(HttpMessageHandlerMock.Object);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(Configuration.HttpClientName))
                .Returns(httpClient);

            return httpClientFactoryMock;
        }

        #endregion
    }
}
