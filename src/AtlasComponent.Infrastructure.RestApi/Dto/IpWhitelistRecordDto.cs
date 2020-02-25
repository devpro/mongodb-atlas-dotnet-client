using System.Collections.Generic;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Dto
{
    public class IpWhitelistRecordDto
    {
        public string CidrBlock { get; set; }
        public string Comment { get; set; }
        public string AwsSecurityGroup { get; set; }
        public string GroupId { get; set; }
        public string IpAddress { get; set; }
        public List<LinkDto> Links { get; set; }
    }
}
