﻿using System.Collections.Generic;
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
        #region Constructor

        public OrganizationRepository(IMongoDbAtlasRestApiConfiguration configuration, ILogger<OrganizationRepository> logger, IHttpClientFactory httpClientFactory, IMapper mapper)
            : base(configuration, logger, httpClientFactory, mapper)
        {
        }

        #endregion

        #region IOrganizationRepository methods

        public async Task<List<OrganizationModel>> FindAllAsync()
        {
            var url = $"{Configuration.BaseUrl}/orgs";
            var resultList = await GetAsync<ResultListDto<OrganizationDto>>(url);
            return Mapper.Map<List<OrganizationModel>>(resultList.Results);
        }

        #endregion
    }
}
