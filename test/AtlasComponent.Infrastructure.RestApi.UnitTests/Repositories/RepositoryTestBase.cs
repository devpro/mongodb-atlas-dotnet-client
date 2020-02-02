using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.UnitTests.Repositories
{
    public abstract class RepositoryTestBase
    {
        #region Protected properties & Constructor

        protected ServiceProvider ServiceProvider { get; private set; }
        protected IMapper Mapper { get; private set; }

        protected RepositoryTestBase()
        {
            Mapper = BuildAutoMapper();
            var services = new ServiceCollection()
                .AddLogging();
            ServiceProvider = services.BuildServiceProvider();
        }

        #endregion

        #region Private fields & Constructor

        protected virtual IMapper BuildAutoMapper()
        {
            var mappingConfig = new MapperConfiguration(x =>
            {
                x.AddProfile(new RestApi.MappingProfiles.GenericMappingProfile());
                x.AllowNullCollections = true;
            });
            return mappingConfig.CreateMapper();
        }

        #endregion
    }
}
