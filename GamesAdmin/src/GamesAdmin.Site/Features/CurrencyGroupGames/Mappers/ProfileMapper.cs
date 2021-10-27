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
            CreateMap<EditViewModel, CurrencyGroupGame>();
        }
    }
}