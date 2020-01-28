namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi
{
    public interface IMongoDbAtlasRestApiConfiguration
    {
        /// <summary>
        /// MongoDB Atlas REST Api base URL.
        /// </summary>
        /// <example>https://cloud.mongodb.com/api/atlas/v1.0</example>
        public string BaseUrl { get; }

        /// <summary>
        /// MongoDB Atlas REST Api public key.
        /// It needs to be created from MongoDB Atlas web interface.
        /// </summary>
        public string PublicKey { get; }

        /// <summary>
        /// MongoDB Atlas REST Api private key associated to the given <see cref="PublicKey"/>.
        /// </summary>
        public string PrivateKey { get; }

        /// <summary>
        /// HTTP client name.
        /// </summary>
        public string HttpClientName { get; }
    }
}
