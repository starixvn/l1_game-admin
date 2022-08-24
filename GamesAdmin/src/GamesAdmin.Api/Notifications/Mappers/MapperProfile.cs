using AutoMapper;
using GamesAdmin.Api.Notifications.Models;
using GamesAdmin.Api.Notifications.Requests;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.Notifications.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AddRequest, NotificationEntity>()
                .ForMember(dest => dest.DisplayKind, opt => opt.MapFrom(src => byte.Parse(src.DisplayKind.Value)));

            CreateMap<UpdateRequest, NotificationEntity>()
                .ForMember(dest => dest.DisplayKind, opt => opt.MapFrom(src => byte.Parse(src.DisplayKind.Value)));

            CreateMap<Notification, NotificationEntity>()
                .ForMember(dest => dest.DisplayKind, opt => opt.MapFrom(src => byte.Parse(src.DisplayKind.Value)));

            CreateMap<CreateAnnouncementModel, NotificationEntity>()
                .ForMember(dest => dest.DisplayKind, opt => opt.MapFrom(src => byte.Parse(src.DisplayKind.Value)));
        }
    }
}