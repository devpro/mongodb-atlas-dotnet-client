using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Exceptions;
using Newtonsoft.Json;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Repositories
{
    public abstract class RepositoryBase
    {
        #region Protected properties & constructors

        protected IMongoDbAtlasRestApiConfiguration Configuration { get; private set; }

        protected ILogger Logger { get; private set; }

        protected IHttpClientFactory HttpClientFactory { get; private set; }

        protected IMapper Mapper { get; private set; }

        protected RepositoryBase(IMongoDbAtlasRestApiConfiguration configuration, ILogger logger, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            Configuration = configuration;
            Logger = logger;
            HttpClientFactory = httpClientFactory;
            Mapper = mapper;
        }

        #endregion

        #region Abstract properties

        protected abstract string ResourceName { get; }

        #endregion

        #region Protected methods

        protected string GenerateUrl(string arguments = "")
        {
            return $"{Configuration.BaseUrl}/{ResourceName}{arguments}";
        }

        protected virtual async Task<T> GetAsync<T>(string url) where T : class
        {
            var client = HttpClientFactory.CreateClient(Configuration.HttpClientName);
            SetDefaultRequestHeaders(client);

            Logger.LogDebug($"Async GET call initiated [HttpRequestUrl={url}]");
            var response = await client.GetAsync(url);
            Logger.LogDebug($"Async GET call completed [HttpRequestUrl={url}] [HttpResponseStatus={response.StatusCode}]");

            var stringResult = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(stringResult))
            {
                throw new ConnectivityException($"Empty response received while calling {url}");
            }

            if (!response.IsSuccessStatusCode)
            {
                Logger.LogDebug($"Status code doesn't indicate success [HttpRequestUrl={url}] [HttpResponseContent={stringResult}]");
                response.EnsureSuccessStatusCode();
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(stringResult);
            }
            catch (Exception exc)
            {
                Logger.LogWarning($"Cannot deserialize GET call response content [HttpRequestUrl={url}] [HttpResponseContent={stringResult}] [SerializationType={typeof(T).ToString()}] [ExceptionMessage={exc.Message}]");
                Logger.LogDebug($"[Stacktrace={exc.StackTrace}]");
                throw new ConnectivityException($"Invalid data received when calling \"{url}\". {exc.Message}.", exc);
            }
        }

        #endregion

        #region Private methods

        private void SetDefaultRequestHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion
    }
}
