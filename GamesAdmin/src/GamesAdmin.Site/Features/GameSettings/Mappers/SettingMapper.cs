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
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Info.Type == 0 ? GameType.AutoRNG : src.Info.Type))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Info.Name))
                .ForMember(dest => dest.RandomMode, opt => opt.MapFrom(src => src.Info.RandomMode))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.EnableSetting.Enabled))
                .ForMember(dest => dest.DisabledMessage, opt => opt.MapFrom(src => src.EnableSetting.DisabledMessage))
                .ForMember(dest => dest.DelayStartTime, opt => opt.MapFrom(src => src.EnableSetting.DelayStartTime))
                .ForMember(dest => dest.BotMaxBet, opt => opt.MapFrom(src => src.BotSetting.BotMaxBet))
                .ForMember(dest => dest.BotEnabled, opt => opt.MapFrom(src => src.BotSetting.BotEnabled))
                .ForMember(dest => dest.HoursMaxBot, opt => opt.MapFrom(src => src.BotSetting.HoursMaxBot))
                .ForMember(dest => dest.SmartBotEnabled, opt => opt.MapFrom(src => src.BotSetting.SmartBotEnabled))
                .ForMember(dest => dest.SmartBotGroupRatio, opt => opt.MapFrom(src => src.BotSetting.SmartBotGroupRatio.ToPercentage()));

            CreateMap<GameConfig, PayoutViewModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Info.Type == 0 ? GameType.AutoRNG : src.Info.Type))
                .ForMember(dest => dest.MinBet, opt => opt.MapFrom(src => src.BetLimit.MinBet))
                .ForMember(dest => dest.MaxBet, opt => opt.MapFrom(src => src.BetLimit.MaxBet))
                .ForMember(dest => dest.Icons, opt => opt.MapFrom(src => src.SlotsConfig.Icons));

            CreateMap<SlotsConfig, JackpotViewModel>()
                .ForMember(dest => dest.WinningRatio, opt => opt.MapFrom(src => src.Jackpot.WinningRatio.ToPercentage()))
                .ForMember(dest => dest.PlayerContribution, opt => opt.MapFrom(src => src.Jackpot.PlayerContributionRatio.ToPercentage()))
                .ForMember(dest => dest.BotContribution, opt => opt.MapFrom(src => src.Jackpot.BotContributionRatio.ToPercentage()))
                .ForMember(dest => dest.ExcludeTestCurrency, opt => opt.MapFrom(src => src.Jackpot.ExcludeTestCurrency))
                .ForMember(dest => dest.JackpotEnabled, opt => opt.MapFrom(src => src.Jackpot.Enabled))
                .ForMember(dest => dest.AllowedCurrency, opt => opt.MapFrom(src => src.Jackpot.AllowedCurrency));

            CreateMap<SlotsConfig, SlotViewModel>()
                .ForMember(dest => dest.Mode, opt => opt.MapFrom(src => (GameMode)src.Mode))
                .ForMember(dest => dest.MinBonus, opt => opt.MapFrom(src => src.Bonus.Min))
                .ForMember(dest => dest.MaxBonus, opt => opt.MapFrom(src => src.Bonus.Max))
                .ForMember(dest => dest.JackpotWinningAmount, opt => opt.MapFrom(src => src.Jackpot.WinningRatio.ToPercentage()))
                .ForMember(dest => dest.PlayerJackpotContribution, opt => opt.MapFrom(src => src.Jackpot.PlayerContributionRatio.ToPercentage()))
                .ForMember(dest => dest.BotJackpotContribution, opt => opt.MapFrom(src => src.Jackpot.BotContributionRatio.ToPercentage()))
                .ForMember(dest => dest.BigWinRatio, opt => opt.MapFrom(src => src.BigWinRatio.ToPercentage()));

            CreateMap<CascadingSlotsConfig, SlotViewModel>()
                .ForMember(dest => dest.Mode, opt => opt.MapFrom(src => (GameMode)src.Mode))
                .ForMember(dest => dest.MinBonus, opt => opt.MapFrom(src => src.MultiplierBonus.Min))
                .ForMember(dest => dest.MaxBonus, opt => opt.MapFrom(src => src.MultiplierBonus.Max))
                .ForMember(dest => dest.BigWinRatio, opt => opt.MapFrom(src => src.BigWinRatio.ToPercentage()));

            CreateMap<GameConfig, StatusItemViewModel>();
            CreateMap<GameConfig, MonitorItemViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Info.Name))
                .ForMember(dest => dest.DBDisplayName, opt => opt.MapFrom(src => src.Info.DisplayName))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Info.Type == 0 ? GameType.AutoRNG : src.Info.Type));

            CreateMap<AddViewModel, SlotsConfig>()
                .ForMember(dest => dest.Mode, opt => opt.MapFrom(src => (byte)src.Mode))
                .ForMember(dest => dest.BigWinRatio, opt => opt.MapFrom(src => src.BigWinRatio.ConvertPercentValue()))
                .ForMember(dest => dest.StakeLevels, opt => opt.MapFrom(src => ConvertStakeLevels(src.Stakes, src.Jackpots, src.BotRatios)))
                .ForMember(dest => dest.Bonus, opt => opt.MapFrom(src => new Bonus(src.MinBonus, src.MaxBonus)))
                .ForMember(dest => dest.Jackpot, opt => opt.MapFrom(src => new Jackpot(
                    src.EnableJackpot,
                    src.WinningRatio.ConvertPercentValue(),
                    src.PlayerJackpotContribution.ConvertPercentValue(),
                    src.BotJackpotContribution.ConvertPercentValue(),
                    false,
                    src.AllowedCurrency)));

            CreateMap<AddViewModel, CascadingSlotsConfig>()
                .ForMember(dest => dest.Mode, opt => opt.MapFrom(src => (byte)src.Mode))
                .ForMember(dest => dest.BigWinRatio, opt => opt.MapFrom(src => src.BigWinRatio.ConvertPercentValue()))
                .ForMember(dest => dest.BetLevels, opt => opt.MapFrom(src => src.BetLevels))
                 .ForMember(dest => dest.BetMultiplier, opt => opt.MapFrom(src => src.BetMultiplier))
                .ForMember(dest => dest.MaxWinPerRound, opt => opt.MapFrom(src => src.MaxWinPerRound))
                .ForMember(dest => dest.MultiplierBonus, opt => opt.MapFrom(src => new MultiplierBonus(src.MinMultiplierBonus, src.MaxMultiplierBonus)));

            CreateMap<AddViewModel, SlotRNGConfig>()
                .ForMember(dest => dest.Mode, opt => opt.MapFrom(src => (byte)src.Mode));

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