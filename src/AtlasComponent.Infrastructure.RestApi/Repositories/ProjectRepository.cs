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
    /// <remarks>https://docs.atlas.mongodb.com/reference/api/projects/</remarks>
    public class ProjectRepository : RepositoryBase, IProjectRepository
    {
        public ProjectRepository(
            IMongoDbAtlasRestApiConfiguration configuration,
            ILogger<ProjectRepository> logger,
            IHttpClientFactory httpClientFactory,
            IMapper mapper)
            : base(configuration, logger, httpClientFactory, mapper)
        {
        }

        /// <remarks>
        /// Seen in MongoDB official documentation:
        ///     "Groups and projects are synonymous terms. Your {GROUP-ID} is the same as your project ID.
        ///     For existing groups, your group/project ID remains the same. The resource and corresponding endpoints use the term groups."
        /// </remarks>
        protected override string ResourceName => "groups";

        public async Task<ProjectModel> FindOneByNameAsync(string name)
        {
            var url = GenerateUrl($"/byName/{name}");
            var result = await GetAsync<ProjectDto>(url);
            return Mapper.Map<ProjectModel>(result);
        }

        public async Task<List<ProjectModel>> FindAllAsync()
        {
            var url = GenerateUrl();
            var resultList = await GetAsync<ResultListDto<ProjectDto>>(url);
            return Mapper.Map<List<ProjectModel>>(resultList.Results);
        }

        public async Task<List<EventModel>> FindAllEventsByProjectIdAsync(string projectId)
        {
            var url = GenerateUrl($"/{projectId}/events");
            var resultList = await GetAsync<ResultListDto<EventDto>>(url);
            return Mapper.Map<List<EventModel>>(resultList.Results);
        }
    }
}
