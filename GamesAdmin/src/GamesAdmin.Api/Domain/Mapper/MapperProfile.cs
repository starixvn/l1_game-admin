namespace GamesAdmin.Api.Domain.Mappers
{
    using AutoMapper;
    using GamesAdmin.Api.Domain.Requests;
    using GamesAdmin.Core.Models;
    using GamesAdmin.Database.Entities;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UpdateDomainRequest, DomainModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));

            CreateMap<DomainEntity, DomainModel>();
            CreateMap<DomainModel, DomainEntity>().ForMember(dest=>dest.Id, opt=>opt.Ignore());
        }
    }
}
