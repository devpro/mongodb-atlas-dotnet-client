using System.Collections.Generic;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Dto
{
    public class ResultListDto<T>
    {
        public List<LinkDto> Links { get; set; }
        public List<T> Results { get; set; }
        public int TotalCount { get; set; }
    }
}
