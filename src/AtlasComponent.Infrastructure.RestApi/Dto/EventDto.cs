using System;
using System.Collections.Generic;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.Dto
{
    public class EventDto
    {
        public string ClusterName { get; set; }
        public DateTime Created { get; set; }
        public string EventTypeName { get; set; }
        public string GroupId { get; set; }
        public string Id { get; set; }
        public bool IsGlobalAdmin { get; set; }
        public List<LinkDto> Links { get; set; }
        public string RemoteAddress { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string TargetUsername { get; set; }
        public string Collection { get; set; }
        public string Database { get; set; }
        public string OpType { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string ReplicaSetName { get; set; }
        public string TargetPublicKey { get; set; }
    }
}
