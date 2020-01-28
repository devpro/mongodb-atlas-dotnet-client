using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Models;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Dto;
using Newtonsoft.Json;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        protected IMongoDbAtlasRestApiConfiguration Configuration { get; private set; }

        protected ILogger Logger { get; private set; }

        protected IHttpClientFactory HttpClientFactory { get; private set; }

        protected IMapper Mapper { get; private set; }

        public OrganizationRepository(IMongoDbAtlasRestApiConfiguration configuration, ILogger logger, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            Configuration = configuration;
            Logger = logger;
            HttpClientFactory = httpClientFactory;
            Mapper = mapper;
        }

        #region IOrganizationRepository methods

        public async Task<List<OrganizationModel>> FindAll()
        {
            var url = $"{Configuration.BaseUrl}/orgs";
            var resultList = await GetAsync<ResultListDto<OrganizationDto>>(url);
            return Mapper.Map<List<OrganizationModel>>(resultList.Results); ;
        }

        #endregion

        protected virtual async Task<T> GetAsync<T>(string url) where T : class
        {
            var client = HttpClientFactory.CreateClient();
            SetDefaultRequestHeaders(client);

            Logger.LogDebug($"Async GET call initiated [HttpRequestUrl={url}]");
            var response = await client.GetAsync(url);
            Logger.LogDebug($"Async GET call completed [HttpRequestUrl={url}] [HttpResponseStatus={response.StatusCode}]");

            var stringResult = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(stringResult))
            {
                throw new Exception($"Empty response received while calling {url}");
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
                throw new Exception($"Invalid data received when calling \"{url}\". {exc.Message}.", exc);
            }
        }

        private void SetDefaultRequestHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
