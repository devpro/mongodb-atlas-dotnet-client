using System;

namespace MongoDb.Atlas.Client.AtlasComponent.Domain.Models
{
    public class ProjectModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OrganizationId { get; set; }
        public int ClusterCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
