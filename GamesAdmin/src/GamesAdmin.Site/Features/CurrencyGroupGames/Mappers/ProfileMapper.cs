using System.Linq;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.Mappers
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<CurrencyGroupGame, EditViewModel>();

            CreateMap<EditViewModel, CurrencyGroupGame>()
                .ForMember(dest => dest.MaxBetPerChoices, opt => opt.MapFrom(src => src.MaxBetChoiceSetting.ToDictionary(setting => setting.Name, setting => setting.MaxBet)));

        }
    }
}