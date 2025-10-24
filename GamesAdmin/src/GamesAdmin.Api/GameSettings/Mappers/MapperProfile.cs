using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.GameSettings.Mappers
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<GameConfigEntity, GameInfo>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));

            CreateMap<GameConfigEntity, GameConfig>()
                .ForMember(dest => dest.Info, opt => opt.MapFrom(src => new GameInfo(
                    src.Name, 
                    src.EnableFreeBet, 
                    (GameType)src.Type, 
                    src.RandomMode, 
                    src.DisplayName, 
                    src.GameId, 
                    src.SiteExclusions, 
                    src.MaxWinPerRound, 
                    src.EnablePromotionRTP,
                    src.ApplyMultiplier, 
                    src.HaveTournament, 
                    src.HaveVideoStream, 
                    src.UseMargin)))
                .ForMember(dest => dest.BetLimit, opt => opt.MapFrom(src => new BetLimit(
                    src.Minbet, 
                    src.Maxbet, 
                    src.MaxBetChoices)))
                .ForMember(dest => dest.BotSetting, opt => opt.MapFrom(src => new BotSetting(
                    src.Botenabled, 
                    src.BotMaxBet, 
                    src.HoursMaxBot, 
                    src.SmartBotEnabled, 
                    src.SmartBotGroupRatio)))
                .ForMember(dest => dest.EnableSetting, opt => opt.MapFrom(src => new EnableSetting(
                    src.Enabled, 
                    src.DelayStartTime, 
                    src.DisabledRound, 
                    src.DisabledMessage)));
        }
    }
}