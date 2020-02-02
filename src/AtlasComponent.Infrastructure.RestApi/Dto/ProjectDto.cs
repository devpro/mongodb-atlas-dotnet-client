using System;
using System.Collections.Generic;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Dto
{
    public class ProjectDto
    {
        public int ClusterCount { get; set; }
        public DateTime Created { get; set; }
        public string Id { get; set; }
        public List<LinkDto> Links { get; set; }
        public string Name { get; set; }
        public string OrgId { get; set; }
    }
}
