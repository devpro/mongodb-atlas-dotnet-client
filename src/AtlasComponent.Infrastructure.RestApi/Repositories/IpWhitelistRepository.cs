using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Models;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories;
using MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Dto;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Repositories
{
    /// <summary>
    /// IP white list repository.
    /// </summary>
    /// <remarks>https://docs.atlas.mongodb.com/reference/api/whitelist/</remarks>
    public class IpWhitelistRepository : RepositoryBase, IIpWhitelistRepository
    {
        public IpWhitelistRepository(IMongoDbAtlasRestApiConfiguration configuration, ILogger<IpWhitelistRepository> logger, IHttpClientFactory httpClientFactory, IMapper mapper)
            : base(configuration, logger, httpClientFactory, mapper)
        {
        }

        protected override string ResourceName => "groups";

        public async Task<List<IpWhitelistRecordModel>> FindAllAsync(string projectId)
        {
            var resultList = await GetAsync<ResultListDto<IpWhitelistRecordDto>>(GenerateUrl($"/{projectId}/whitelist"));
            return Mapper.Map<List<IpWhitelistRecordModel>>(resultList.Results);
        }
    }
}
