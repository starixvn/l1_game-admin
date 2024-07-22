using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Database.Entities.Promotion;
using MongoDB.Driver.Core.Authentication;

namespace GamesAdmin.Api.Promotions.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<PromotionEntity, PromotionModel>()
                .ForMember(x => x.PromotionId, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => (GameId)src.GameId));

            CreateMap<PromotionModel, PromotionEntity>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => (int)src.GameId));

            CreateMap<PromotionConfigEntity, PromotionConfigModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<Operator, OperatorModel>();
            CreateMap<CurrencyGroup, CurrencyGroupsModel>();
            CreateMap<WinningPrize, WinningPrizeModel>();
            CreateMap<PromotionLeaderboardEntity, AwardWinningPlayer>()
                .ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.PointInBaseCurrency));

            CreateMap<PromotionConfigModel, PromotionConfigEntity>()   
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<OperatorModel, Operator>();
            CreateMap<CurrencyGroupsModel, CurrencyGroup>();
            CreateMap<WinningPrizeModel, WinningPrize>();
        }
    }
}