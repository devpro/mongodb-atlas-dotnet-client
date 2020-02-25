using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi;

namespace MongoDb.Atlas.Client.ConsoleApp.UnitTests.Fakes
{
    public class AppConfigurationFake : IMongoDbAtlasRestApiConfiguration
    {
        public string BaseUrl => "http://console.does.not.exist";

        public string PublicKey => "helloconsole";

        public string PrivateKey => "thereconsole";

        public string HttpClientName => "fakeconsole";
    }
}
