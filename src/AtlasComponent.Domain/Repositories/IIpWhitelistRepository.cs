using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDb.Atlas.Client.AtlasComponent.Domain.Models;

namespace MongoDb.Atlas.Client.AtlasComponent.Domain.Repositories
{
    public interface IIpWhitelistRepository
    {
        Task<List<IpWhitelistRecordModel>> FindAllAsync(string projectId);

        Task<List<IpWhitelistRecordModel>> CreateAsync(string projectId, List<IpWhitelistRecordModel> input);
    }
}
