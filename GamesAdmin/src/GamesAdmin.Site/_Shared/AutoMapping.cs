using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.GameSettings.ViewModels;

namespace GamesAdmin.Site._Shared
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<GameConfig, EditViewModel>(); 
        }
    }
}
