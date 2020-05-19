using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.UmSchedule.Requests;
using GamesAdmin.Site.Features.Dashboard.ViewModels;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using GamesAdmin.Core.Models.Um;

namespace GamesAdmin.Site._Shared
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<GameConfig, EditViewModel>();
            CreateMap<GameConfig, StatusItemViewModel>();
            CreateMap<OnlineUser, OnlineUserViewModel>();
            CreateMap<BotUser, BotUserViewModel>();
            CreateMap<CreateRequest, UmSchedule>();
        }
    }
}
