using System.Collections.Generic;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features.GameSettings.ViewModels;

namespace GamesAdmin.Site.Features.GameSettings
{
    public class SettingMapper : Profile
    {
        public SettingMapper()
        {
            CreateMap<GameConfig, EditViewModel>();
            CreateMap<GameConfig, StatusItemViewModel>();
            CreateMap<GameConfig, MonitorItemViewModel>();            

            CreateMap<AddViewModel, SlotsConfig>()
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => (byte)src.Difficulty))
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