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

            CreateMap<Dto.ProjectDto, Domain.Models.ProjectModel>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.Created))
                .ForMember(x => x.OrganizationId, opt => opt.MapFrom(x => x.OrgId));
            CreateMap<Domain.Models.ProjectModel, Dto.ProjectDto>()
                .ForMember(x => x.Created, opt => opt.MapFrom(x => x.CreatedAt))
                .ForMember(x => x.OrgId, opt => opt.MapFrom(x => x.OrganizationId))
                .ForMember(x => x.Links, opt => opt.Ignore());

            CreateMap<Dto.EventDto, Domain.Models.EventModel>();
            CreateMap<Domain.Models.EventModel, Dto.EventDto>()
                .ForMember(x => x.Links, opt => opt.Ignore());

            CreateMap<Dto.IpWhitelistRecordDto, Domain.Models.IpWhitelistRecordModel>();
            CreateMap<Domain.Models.IpWhitelistRecordModel, Dto.IpWhitelistRecordDto>()
                .ForMember(x => x.AwsSecurityGroup, opt => opt.Ignore())
                .ForMember(x => x.DeleteAfterDate, opt => opt.Ignore())
                .ForMember(x => x.Links, opt => opt.Ignore());
        }
    }
}
