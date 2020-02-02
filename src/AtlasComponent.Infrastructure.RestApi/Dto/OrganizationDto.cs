
using System.Collections.Generic;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Dto
{
    public class OrganizationDto
    {
        public string Id { get; set; }
        public List<LinkDto> Links { get; set; }
        public string Name { get; set; }
    }
}
