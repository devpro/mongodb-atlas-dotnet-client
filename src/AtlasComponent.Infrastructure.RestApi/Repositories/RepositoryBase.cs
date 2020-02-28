using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Exceptions;
using Withywoods.Serialization.Json;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Repositories
{
    /// <summary>
    /// Abtract class for all repositories.
    /// </summary>
    public abstract class RepositoryBase
    {
        protected RepositoryBase(IMongoDbAtlasRestApiConfiguration configuration, ILogger logger, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            Configuration = configuration;
            Logger = logger;
            HttpClientFactory = httpClientFactory;
            Mapper = mapper;
        }

        protected IMongoDbAtlasRestApiConfiguration Configuration { get; private set; }

        protected ILogger Logger { get; private set; }

        protected IHttpClientFactory HttpClientFactory { get; private set; }

        protected IMapper Mapper { get; private set; }

        protected abstract string ResourceName { get; }

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
                return stringResult.FromJson<T>();
            }
            catch (Exception exc)
            {
                Logger.LogWarning($"Cannot deserialize GET call response content [HttpRequestUrl={url}] [HttpResponseContent={stringResult}] [SerializationType={typeof(T).ToString()}] [ExceptionMessage={exc.Message}]");
                Logger.LogDebug($"[Stacktrace={exc.StackTrace}]");
                throw new ConnectivityException($"Invalid data received when calling \"{url}\". {exc.Message}.", exc);
            }
        }

        protected virtual async Task<T> PostAsync<T>(string url, object body, string httpClientName = null) where T : class
        {
            var client = HttpClientFactory.CreateClient(Configuration.HttpClientName);
            SetDefaultRequestHeaders(client);

            Logger.LogDebug($"Async POST call initiated [HttpRequestUrl={url}]");
            var response = await client.PostAsync(url, new StringContent(body.ToJson(), Encoding.UTF8, "application/json"));
            Logger.LogDebug($"Async POST call completed [HttpRequestUrl={url}] [HttpResponseStatus={response.StatusCode}]");

            var stringResult = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(stringResult))
            {
                throw new ConnectivityException($"Empty response received while calling {url}");
            }

            if (!response.IsSuccessStatusCode)
            {
                Logger.LogDebug($"Status code doesn't indicate success [HttpRequestUrl={url}] [HttpStatusCode={response.StatusCode}] [HttpResponseContent={stringResult}]");
                response.EnsureSuccessStatusCode();
            }

            try
            {
                return stringResult.FromJson<T>();
            }
            catch (Exception exc)
            {
                Logger.LogWarning($"Cannot deserialize POST call response content [HttpRequestUrl={url}] [HttpResponseContent={stringResult}] [SerializationType={typeof(T).ToString()}] [ExceptionMessage={exc.Message}]");
                Logger.LogDebug($"[Stacktrace={exc.StackTrace}]");
                throw new ConnectivityException($"Invalid data received when calling \"{url}\". {exc.Message}.", exc);
            }
        }

        protected virtual async Task PutAsync(string url, object body)
        {
            var client = HttpClientFactory.CreateClient(Configuration.HttpClientName);
            SetDefaultRequestHeaders(client);

            Logger.LogDebug($"Async PUT call initiated [HttpRequestUrl={url}]");
            var response = await client.PutAsync(url, new StringContent(body.ToJson(), Encoding.UTF8, "application/json"));
            Logger.LogDebug($"Async PUT call completed [HttpRequestUrl={url}] [HttpResponseStatus={response.StatusCode}]");

            if (!response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                Logger.LogDebug($"Status code doesn't indicate success [HttpRequestUrl={url}] [HttpStatusCode={response.StatusCode}] [HttpResponseContent={stringResult}]");
                response.EnsureSuccessStatusCode();
            }
        }

        protected virtual async Task DeleteAsync(string url)
        {
            var client = HttpClientFactory.CreateClient(Configuration.HttpClientName);
            SetDefaultRequestHeaders(client);

            Logger.LogDebug($"Async DELETE call initiated [HttpRequestUrl={url}]");
            var response = await client.DeleteAsync(url);
            Logger.LogDebug($"Async DELETE call completed [HttpRequestUrl={url}] [HttpResponseStatus={response.StatusCode}]");

            if (!response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                Logger.LogDebug($"Status code doesn't indicate success [HttpRequestUrl={url}] [HttpStatusCode={response.StatusCode}] [HttpResponseContent={stringResult}]");
                response.EnsureSuccessStatusCode();
            }
        }

        private void SetDefaultRequestHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
