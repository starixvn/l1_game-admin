namespace GamesAdmin.Site.Features.Domain
{
    using AutoMapper;
    using GamesAdmin.Core.Models;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UpdateDomainRequest, DomainModel>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
