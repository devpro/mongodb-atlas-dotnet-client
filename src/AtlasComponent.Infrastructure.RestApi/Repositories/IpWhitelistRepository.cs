using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var url = GenerateUrl($"/{projectId}/whitelist");
            var resultList = await GetAsync<ResultListDto<IpWhitelistRecordDto>>(url);
            return Mapper.Map<List<IpWhitelistRecordModel>>(resultList.Results);
        }

        public async Task<List<IpWhitelistRecordModel>> CreateAsync(string projectId, List<IpWhitelistRecordModel> input)
        {
            var url = GenerateUrl($"/{projectId}/whitelist");
            var created = await PostAsync<ResultListDto<IpWhitelistRecordDto>>(url,
                input.Select(x => new { cidrBlock = x.CidrBlock, comment = x.Comment }));
            return Mapper.Map<List<IpWhitelistRecordModel>>(created.Results);
        }

        public async Task DeleteAsync(string projectId, IpWhitelistRecordModel input)
        {
            var url = GenerateUrl($"/{projectId}/whitelist/{WebUtility.UrlEncode(input.CidrBlock)}");
            await DeleteAsync(url);
        }
    }
}
