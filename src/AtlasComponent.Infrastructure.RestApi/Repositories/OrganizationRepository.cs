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
    public class OrganizationRepository : RepositoryBase, IOrganizationRepository
    {
        public OrganizationRepository(
            IMongoDbAtlasRestApiConfiguration configuration,
            ILogger<OrganizationRepository> logger,
            IHttpClientFactory httpClientFactory,
            IMapper mapper)
            : base(configuration, logger, httpClientFactory, mapper)
        {
        }

        protected override string ResourceName => "orgs";

        public async Task<List<OrganizationModel>> FindAllAsync()
        {
            var resultList = await GetAsync<ResultListDto<OrganizationDto>>(GenerateUrl());
            return Mapper.Map<List<OrganizationModel>>(resultList.Results);
        }
    }
}
