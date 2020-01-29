using System.Collections.Generic;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Dto
{
    public class ResultListDto<T>
    {
        public List<LinkDto> Links { get; set; }
        public List<T> Results { get; set; }
        public int TotalCount { get; set; }
    }

    public class LinkDto
    {
        public string Href { get; set; }
        public string Rel { get; set; }
    }

    public class OrganizationDto
    {
        public string Id { get; set; }
        public List<LinkDto> Links { get; set; }
        public string Name { get; set; }
    }
}
