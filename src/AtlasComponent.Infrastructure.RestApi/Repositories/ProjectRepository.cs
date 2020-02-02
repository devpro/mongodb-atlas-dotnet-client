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
    /// Project repository.
    /// </summary>
    /// <see cref="https://docs.atlas.mongodb.com/reference/api/projects/"/>
    public class ProjectRepository : RepositoryBase, IProjectRepository
    {
        #region Constructor

        public ProjectRepository(IMongoDbAtlasRestApiConfiguration configuration, ILogger<ProjectRepository> logger, IHttpClientFactory httpClientFactory, IMapper mapper)
            : base(configuration, logger, httpClientFactory, mapper)
        {
        }

        #endregion

        #region RepositoryBase Properties

        /// <remarks>
        /// Seen in MongoDB official documentation:
        ///     "Groups and projects are synonymous terms. Your {GROUP-ID} is the same as your project ID.
        ///     For existing groups, your group/project ID remains the same. The resource and corresponding endpoints use the term groups."
        /// </remarks>
        protected override string ResourceName => "groups";

        #endregion

        #region IProjectRepository Methods

        public async Task<List<ProjectModel>> FindAllAsync()
        {
            var resultList = await GetAsync<ResultListDto<ProjectDto>>(GenerateUrl());
            return Mapper.Map<List<ProjectModel>>(resultList.Results);
        }

        public async Task<List<EventModel>> FindAllEventsByProjectIdAsync(string projectId)
        {
            var resultList = await GetAsync<ResultListDto<EventDto>>(GenerateUrl($"/{projectId}/events"));
            return Mapper.Map<List<EventModel>>(resultList.Results);
        }

        public async Task<List<WhiteListIpModel>> FindAllWhiteListIpAddressesByProjectIdAsync(string projectId)
        {
            var resultList = await GetAsync<ResultListDto<WhiteListIpDto>>(GenerateUrl($"/{projectId}/whitelist"));
            return Mapper.Map<List<WhiteListIpModel>>(resultList.Results);
        }

        #endregion
    }
}
