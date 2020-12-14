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
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.EnableSetting.Enabled))
                .ForMember(dest => dest.DisabledMessage, opt => opt.MapFrom(src => src.EnableSetting.DisabledMessage))
                .ForMember(dest => dest.DelayStartTime, opt => opt.MapFrom(src => src.EnableSetting.DelayStartTime))
                .ForMember(dest => dest.BotMaxBet, opt => opt.MapFrom(src => src.BotSetting.BotMaxBet))
                .ForMember(dest => dest.BotEnabled, opt => opt.MapFrom(src => src.BotSetting.BotEnabled))
                .ForMember(dest => dest.HoursMaxBot, opt => opt.MapFrom(src => src.BotSetting.HoursMaxBot));

            CreateMap<GameConfig, PayoutViewModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Info.Type))
                .ForMember(dest => dest.MinBet, opt => opt.MapFrom(src => src.BetLimit.MinBet))
                .ForMember(dest => dest.MaxBet, opt => opt.MapFrom(src => src.BetLimit.MaxBet))
                .ForMember(dest => dest.Icons, opt => opt.MapFrom(src => src.SlotsConfig.Icons));

            CreateMap<SlotsConfig, JackpotViewModel>()               
                .ForMember(dest => dest.WinningRatio, opt => opt.MapFrom(src => src.Jackpot.WinningRatio.ToPercentage()))
                .ForMember(dest => dest.PlayerContribution, opt => opt.MapFrom(src => src.Jackpot.PlayerContributionRatio.ToPercentage()))
                .ForMember(dest => dest.BotContribution, opt => opt.MapFrom(src => src.Jackpot.BotContributionRatio.ToPercentage()))
                .ForMember(dest => dest.ExcludeUUS, opt => opt.MapFrom(src => src.Jackpot.ExcludeUUS))
                .ForMember(dest => dest.JackpotEnabled, opt => opt.MapFrom(src => src.Jackpot.Enabled));

            CreateMap<SlotsConfig, SlotViewModel>()
                .ForMember(dest => dest.Mode, opt => opt.MapFrom(src => (GameMode)src.Mode))
                .ForMember(dest => dest.MinBonus, opt => opt.MapFrom(src => src.Bonus.Min))
                .ForMember(dest => dest.MaxBonus, opt => opt.MapFrom(src => src.Bonus.Max))
                .ForMember(dest => dest.JackpotWinningAmount, opt => opt.MapFrom(src => src.Jackpot.WinningRatio.ToPercentage()))
                .ForMember(dest => dest.PlayerJackpotContribution, opt => opt.MapFrom(src => src.Jackpot.PlayerContributionRatio.ToPercentage()))
                .ForMember(dest => dest.BotJackpotContribution, opt => opt.MapFrom(src => src.Jackpot.BotContributionRatio.ToPercentage()))
                .ForMember(dest => dest.BigWinRatio, opt => opt.MapFrom(src => src.BigWinRatio.ToPercentage()));

            CreateMap<GameConfig, StatusItemViewModel>();
            CreateMap<GameConfig, MonitorItemViewModel>();

            CreateMap<AddViewModel, SlotsConfig>()
                .ForMember(dest => dest.Mode, opt => opt.MapFrom(src => (byte)src.Mode))
                .ForMember(dest => dest.StakeLevels, opt => opt.MapFrom(src => ConvertStakeLevels(src.Stakes, src.Jackpots, src.BotRatios)))
                .ForMember(dest => dest.Bonus, opt => opt.MapFrom(src => new Bonus(src.MinBonus, src.MaxBonus)))
                .ForMember(dest => dest.Jackpot, opt => opt.MapFrom(src => new Jackpot(
                    src.EnableJackpot, 
                    src.JackpotRTP.ConvertPercentValue(), 
                    src.PlayerJackpotContribution.ConvertPercentValue(), 
                    src.BotJackpotContribution.ConvertPercentValue())));

            CreateMap<AddViewModel, GameInfo>();
            CreateMap<AddViewModel, EnableSetting>();
            CreateMap<AddViewModel, BotSetting>();
            CreateMap<AddViewModel, BetLimit>();

            CreateMap<AddViewModel, Bonus>();
        }

        private static List<StakeLevel> ConvertStakeLevels(double[] stakes, double[] jackpots, double[] botRatios)
        {
            var stakeLevels = new List<StakeLevel>();

            for (var i = 0; i < stakes.Length; i++)
            {
                stakeLevels.Add(new StakeLevel(stakes[i], jackpots[i], botRatios[i].ConvertPercentValue()));
            }

            return stakeLevels;
        }
    }
}