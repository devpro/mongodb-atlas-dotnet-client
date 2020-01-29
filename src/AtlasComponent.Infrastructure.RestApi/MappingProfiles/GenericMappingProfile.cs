using AutoMapper;

namespace MongoDb.Atlas.Client.AtlasComponent.Infrastructure.RestApi.MappingProfiles
{
    public class GenericMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "MongoDbAtlasClientAtlasComponentRestApiGenericMappingProfile"; }
        }

        public GenericMappingProfile()
        {
            CreateMap<Dto.OrganizationDto, Domain.Models.OrganizationModel>();
            CreateMap<Domain.Models.OrganizationModel, Dto.OrganizationDto>()
                .ForMember(x => x.Links, opt => opt.Ignore());
        }
    }
}
