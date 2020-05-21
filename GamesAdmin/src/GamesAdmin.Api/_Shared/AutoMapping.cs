using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamesAdmin.Api.Notifications.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Um;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;

namespace GamesAdmin.Api._Shared
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<List<ObjectId>, List<string>>().ConvertUsing(o => o.Select(os => os.ToString()).ToList());
            CreateMap<List<string>, List<ObjectId>>().ConvertUsing(o => o.Select(os => ObjectId.Parse(os)).ToList());
            CreateMap<ObjectId, string>().ConvertUsing(o => o.ToString());
            CreateMap<string, ObjectId>().ConvertUsing(s => ObjectId.Parse(s));

            CreateMap<GameConfigEntity, GameConfig>()
                 .ForMember(dest => dest.DisabledRound, opt => opt.MapFrom(src => src.Disabledround));

            CreateMap<GameConfig, GameConfigEntity>();

            CreateMap<BigSmallRoundEntity, Round>();
            CreateMap<BigSmallOnesEntity, BetInfo>();

            CreateMap<User, AdminUserEntity>();
            CreateMap<UmSchedule, EventScheduleEntity>()
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start.UtcDateTime))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End.UtcDateTime))
                .ReverseMap();

            CreateMap<AddRequest, NotificationEntity>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => byte.Parse(src.EventType.Value)))
                .ForMember(dest => dest.DisplayKind, opt => opt.MapFrom(src => byte.Parse(src.DisplayKind.Value)));

            CreateMap<Notification, NotificationEntity>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => byte.Parse(src.EventType.Value)))
                .ForMember(dest => dest.DisplayKind, opt => opt.MapFrom(src => byte.Parse(src.DisplayKind.Value)));

            CreateMap<NotificationEntity, Notification>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => Enumeration.FromValue<EventScheduleType>(src.EventType.ToString())))
                .ForMember(dest => dest.DisplayKind, opt => opt.MapFrom(src => Enumeration.FromValue<NotificationDisplayKind>(src.DisplayKind.ToString())));
        }
    }
}
