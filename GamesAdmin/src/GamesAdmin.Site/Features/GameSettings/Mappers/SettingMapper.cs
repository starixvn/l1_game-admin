using System.Collections.Generic;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Extensions;
using GamesAdmin.Site.Features.GameSettings.ViewModels;

namespace GamesAdmin.Site.Features.GameSettings
{
    public class SettingMapper : Profile
    {
        public SettingMapper()
        {
            CreateMap<GameConfig, EditViewModel>()
                .ForMember(dest => dest.EnableFreeBet, opt => opt.MapFrom(src => src.Info.EnableFreeBet))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Info.Type))
                .ForMember(dest => dest.MinBet, opt => opt.MapFrom(src => src.BetLimit.MinBet))
                .ForMember(dest => dest.MaxBet, opt => opt.MapFrom(src => src.BetLimit.MaxBet))
                .ForMember(dest => dest.MaxBetChoices, opt => opt.MapFrom(src => src.BetLimit.MaxBetChoices))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.EnableSetting.Enabled))
                .ForMember(dest => dest.DisabledMessage, opt => opt.MapFrom(src => src.EnableSetting.DisabledMessage))
                .ForMember(dest => dest.DelayStartTime, opt => opt.MapFrom(src => src.EnableSetting.DelayStartTime))
                .ForMember(dest => dest.BotMaxBet, opt => opt.MapFrom(src => src.BotSetting.BotMaxBet))
                .ForMember(dest => dest.BotEnabled, opt => opt.MapFrom(src => src.BotSetting.BotEnabled))
                .ForMember(dest => dest.HoursMaxBot, opt => opt.MapFrom(src => src.BotSetting.HoursMaxBot))
                .ForMember(dest => dest.MinBonus, opt => opt.MapFrom(src => src.SlotsConfig.Bonus.Min))
                .ForMember(dest => dest.MaxBonus, opt => opt.MapFrom(src => src.SlotsConfig.Bonus.Max))
                .ForMember(dest => dest.JackpotRTP, opt => opt.MapFrom(src => src.SlotsConfig.Jackpot.RTP))
                .ForMember(dest => dest.PlayerJackpotContribution, opt => opt.MapFrom(src => src.SlotsConfig.Jackpot.PlayerContributionRatio))
                .ForMember(dest => dest.BotJackpotContribution, opt => opt.MapFrom(src => src.SlotsConfig.Jackpot.BotContributionRatio))
                .ForMember(dest => dest.Mode, opt => opt.MapFrom(src => (GameMode)src.SlotsConfig.Mode))
                .ForMember(dest => dest.BigWinRatio, opt => opt.MapFrom(src => src.SlotsConfig.BigWinRatio.Round()));

            CreateMap<GameConfig, StatusItemViewModel>();
            CreateMap<GameConfig, MonitorItemViewModel>();

            CreateMap<AddViewModel, SlotsConfig>()
                .ForMember(dest => dest.Mode, opt => opt.MapFrom(src => (byte)src.Mode))
                .ForMember(dest => dest.StakeLevels, opt => opt.MapFrom(src => ConvertStakeLevels(src.Stakes, src.Jackpots)))
                .ForMember(dest => dest.Bonus, opt => opt.MapFrom(src => new Bonus(src.MinBonus, src.MaxBonus)))
                .ForMember(dest => dest.Jackpot, opt => opt.MapFrom(src => new Jackpot(src.EnableJackpot, src.JackpotRTP, src.PlayerJackpotContribution, src.BotJackpotContribution)));

            CreateMap<AddViewModel, GameInfo>();
            CreateMap<AddViewModel, EnableSetting>();
            CreateMap<AddViewModel, BotSetting>();
            CreateMap<AddViewModel, BetLimit>();

            CreateMap<AddViewModel, Bonus>();
        }

        private static List<StakeLevel> ConvertStakeLevels(double[] stakes, double[] jackpots)
        {
            var stakeLevels = new List<StakeLevel>();

            for (var i = 0; i < stakes.Length; i++)
            {
                stakeLevels.Add(new StakeLevel(stakes[i], jackpots[i]));
            }

            return stakeLevels;
        }
    }
}