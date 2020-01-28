using Microsoft.Extensions.Configuration;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi;

namespace MongoDb.Atlas.Client.ConsoleApp
{
    /// <summary>
    /// Application configuration.
    /// </summary>
    public class AppConfiguration : IMongoDbAtlasRestApiConfiguration
    {
        #region Constructor & private fields

        private readonly IConfigurationRoot _configurationRoot;

        public AppConfiguration(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        #endregion

        #region IMongoDbAtlasRestApiConfiguration Properties

        string IMongoDbAtlasRestApiConfiguration.BaseUrl => "https://cloud.mongodb.com/api/atlas/v1.0";

        string IMongoDbAtlasRestApiConfiguration.PublicKey => _configurationRoot.GetSection("Infrastructure:MongoDb:Atlas:Api:PublicKey").Value;

        string IMongoDbAtlasRestApiConfiguration.PrivateKey => _configurationRoot.GetSection("Infrastructure:MongoDb:Atlas:Api:PrivateKey").Value;

        #endregion
    }
}
