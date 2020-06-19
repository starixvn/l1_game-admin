using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.UmSchedule.Requests;
using GamesAdmin.Site.Features.Dashboard.ViewModels;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using GamesAdmin.Core.Models.Um;
using GamesAdmin.Site.Features.UmSchedule.ViewModels;
using GamesAdmin.Site.Features.Notifications.Requests;

namespace GamesAdmin.Site._Shared
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<GameConfig, Features.GameSettings.ViewModels.EditViewModel>();
            CreateMap<GameConfig, StatusItemViewModel>();
            CreateMap<GameConfig, MonitorItemViewModel>();
            CreateMap<OnlineUser, OnlineUserViewModel>();
            CreateMap<BotUser, BotUserViewModel>();
            CreateMap<CreateRequest, UmSchedule>();

            CreateMap<UmSchedule, UmScheduleItemViewModel>();

            CreateMap<AddRequest, Notification>();
            CreateMap<UpdateRequest, Notification>();
        }
    }
}
