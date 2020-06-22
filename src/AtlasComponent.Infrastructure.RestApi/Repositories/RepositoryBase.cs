using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Withywoods.Net.Http;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Repositories
{
    /// <summary>
    /// Abtract class for all repositories.
    /// </summary>
    public abstract class RepositoryBase : HttpRepositoryBase
    {
        protected RepositoryBase(
            IMongoDbAtlasRestApiConfiguration configuration,
            ILogger logger,
            IHttpClientFactory httpClientFactory,
            IMapper mapper)
            : base(logger, httpClientFactory)
        {
            Configuration = configuration;
            Mapper = mapper;
        }

        protected IMongoDbAtlasRestApiConfiguration Configuration { get; private set; }

        protected IMapper Mapper { get; private set; }

        protected override string HttpClientName => Configuration.HttpClientName;

        protected abstract string ResourceName { get; }

        protected string GenerateUrl(string arguments = "")
        {
            return $"{Configuration.BaseUrl}/{ResourceName}{arguments}";
        }

        protected virtual async Task DeleteAsync(string url)
        {
            var client = HttpClientFactory.CreateClient(Configuration.HttpClientName);

            Logger.LogDebug($"Async DELETE call initiated [HttpRequestUrl={url}]");
            var response = await client.DeleteAsync(url);
            Logger.LogDebug($"Async DELETE call completed [HttpRequestUrl={url}] [HttpResponseStatus={response.StatusCode}]");

            if (!response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                Logger.LogWarning($"Status code doesn't indicate success [HttpRequestUrl={url}] [HttpStatusCode={response.StatusCode}] [HttpResponseContent={stringResult}]");
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
