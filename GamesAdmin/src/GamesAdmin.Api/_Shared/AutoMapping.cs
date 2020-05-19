using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Um;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

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
            CreateMap<UmSchedule, EventScheduleEntity>();
            CreateMap<EventScheduleEntity, UmSchedule>();
        }
    }
}
