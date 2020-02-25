namespace MongoDb.Atlas.Client.AtlasComponent.Domain.Models
{
    public class IpWhitelistRecordModel
    {
        public string IpAddress { get; set; }
        public string CidrBlock { get; set; }
        public string Comment { get; set; }
        public string GroupId { get; set; }
    }
}
