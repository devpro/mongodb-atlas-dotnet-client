namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.UnitTests.Fakes
{
    public class FakeConfiguration : IMongoDbAtlasRestApiConfiguration
    {
        public string BaseUrl => "http://does.not.exist";

        public string PublicKey => "hello";

        public string PrivateKey => "there";

        public string HttpClientName => "fake";
    }
}
