using AutoMapper;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.Partners.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<PartnerEntity, Partner>();

            CreateMap<Partner, PartnerEntity>();
        }
    }
}