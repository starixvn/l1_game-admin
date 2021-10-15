using AutoMapper;
using GamesAdmin.Core.Models.Partner;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.Partner.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<PartnerEntity, PartnerInfo>();

            CreateMap<PartnerInfo, PartnerEntity>();
        }
    }
}