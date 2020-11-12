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
            CreateMap<AutoRNGGameConfig, EditViewModel>();
            CreateMap<AutoRNGGameConfig, StatusItemViewModel>();
            CreateMap<AutoRNGGameConfig, MonitorItemViewModel>();
            CreateMap<AddViewModel, Requests.SlotsConfigRequest>()
                .ForMember(dest => dest.Stakes, opt => opt.MapFrom(src => ConvertStakeLevels(src.Stakes, src.Jackpots)));

            CreateMap<AddViewModel, GameInfo>();
            CreateMap<AddViewModel, EnableSetting>();
            CreateMap<AddViewModel, BotSetting>();
            CreateMap<AddViewModel, BetLimit>();
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