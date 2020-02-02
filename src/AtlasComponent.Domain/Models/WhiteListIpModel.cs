namespace MongoDb.Atlas.Client.AtlasComponent.Domain.Models
{
    public class WhiteListIpModel
    {
        public string CidrBlock { get; set; }
        public string Comment { get; set; }
        public string GroupId { get; set; }
        public string IpAddress { get; set; }
    }
}
