namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.UnitTests
{
    public class DummyMongoDbAtlasRestApiConfiguration : IMongoDbAtlasRestApiConfiguration
    {
        public string BaseUrl => "https://dummy.mongodb.com/api/atlas/v1.0";

        public string PublicKey => "publickey";

        public string PrivateKey => "verysecretprivatekey";

        public string HttpClientName => "dummyclient";
    }
}
