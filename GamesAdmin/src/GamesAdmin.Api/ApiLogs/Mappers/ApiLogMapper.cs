using System;
using AutoMapper;
using GamesAdmin.Api.ApiLogs.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models.ApiLogs;
using GamesAdmin.Database.ApiLogs.Entities;

namespace GamesAdmin.Api.ApiLogs.Mappers
{
    public class ApiLogMapper : Profile
    {
        public ApiLogMapper()
        {
            CreateMap<QueryLogModel, QueryLogRequest>()
                .ConstructUsing(model => new QueryLogRequest(
                    (ApiLogType)model.LogType,
                    model.PartnerId,
                    model.FromDate,
                    model.ToDate,
                    StringFormatExtensions.FormatPlayerId(model.SiteId, model.ObCustId),
                    model.GameRoundId,
                    model.GameTypeId,
                    model.Username));

            CreateMap<SBLogEntity, SBLog>()
                .ConstructUsing((entity, context) => new SBLog(
                    entity.LogTimeStamp,
                    entity.RequestTimeStamp,
                    MapRequest<IApiLogRequest>(context, entity.Request, typeof(SBRequestEntity), typeof(SBRequest)),
                    entity.ResponseTimeStamp,
                    MapResult<ISBResult>(context, entity.Result, typeof(SBResultEntity), typeof(SBResult)),
                    MapArchive(context, entity.Archive)));

            CreateMap<PartnerLogEntity, PartnerLog>()
                .ConstructUsing((entity, context) => new PartnerLog(
                    entity.PartnerId,
                    entity.OperatorId,
                    entity.LogTimeStamp,
                    entity.RequestTimeStamp,
                    MapRequest<IApiLogRequest>(context, entity.Call, typeof(PartnerCallEntity), typeof(PartnerCall)),
                    entity.ResponseTimeStamp,
                    MapResult<IPartnerResult>(context, entity.Result, typeof(PartnerResultEntity), typeof(PartnerResult)),
                    entity.System,
                    entity.AccessKey,
                    MapArchive(context, entity.Archive)));

            CreateMap<TicketDetailLogEntitty, TicketDetailLog>()
                .ConstructUsing((entity, context) => new TicketDetailLog(
                    entity.LogTimeStamp,
                    entity.RequestTimeStamp,
                    MapRequest<IApiLogRequest>(context, entity.Call, typeof(TicketDetailRequestEntity), typeof(TicketDetailRequest)),
                    entity.ResponseTimeStamp,
                    MapResult<ISBResult>(context, entity.Result, typeof(TicketDetailResultEntity), typeof(TicketDetailResult)),
                    MapArchive(context, entity.Archive)));

            CreateMap<PartnerCallEntity, PartnerEndGameCall>();
            CreateMap<PartnerCallEntity, PartnerPlaceBetCall>();
            CreateMap<PartnerCallEntity, PartnerVoidGameCall>();
            CreateMap<PartnerCallEntity, PartnerCall>();

            CreateMap<PartnerResultEntity, PartnerResult>();
            CreateMap<PartnerResultEntity, PartnerPlaceBetResult>();

            CreateMap<SBRequestEntity, SignInRequest>();
            CreateMap<SBRequestEntity, EndGameInfoRequest>();
            CreateMap<SBRequestEntity, SBRequest>();

            CreateMap<SBResultEntity, SingleSignOnResult>();
            CreateMap<SBResultEntity, RetrieveEndGameInfoResult>();
            CreateMap<SBResultEntity, SBResult>();

            CreateMap<TicketDetailRequestEntity, TicketDetailRequest>();
            CreateMap<TicketDetailRequestEntity, SBRequest>();

            CreateMap<TicketDetailResultEntity, TicketDetailResult>();
            CreateMap<TicketDetailResultEntity, SBResult>();

            CreateMap<ApiLogArchiveEntity, ApiLogArchive>();
            CreateMap<ApiLogArchiveEntity, BetLogArchive>();

            CreateMap<MemberSignInLogEntity, MemberSignInLog>()
                 .ForMember(x => x.Archive, opt => opt.MapFrom((source, dest, member, context) => MapArchive(context, source.Archive)));
            CreateMap<MemberSignInRequestEntity, MemberSignInRequest>();
            CreateMap<MemberSignInResultEntity, MemberSignInResult>();
            CreateMap<PublicUserInfoEntity, PublicUserInfo>();
            CreateMap<UserInfoEntity, Core.Models.ApiLogs.UserInfo>();
            CreateMap<PartnerCallEntity, MemberFailedSignInRequest>();
            CreateMap<PartnerResultEntity, MemberFailedSignInResult>();

            CreateMap<OWSignInLogEntity, OWSignInLog>()
                .ForMember(x => x.Archive, opt => opt.MapFrom((source, dest, member, context) => MapArchive(context, source.Archive)));
            CreateMap<OWSignInRequestEntity, OWSignInRequest>();
            CreateMap<OWSignInResultEntity, OWSignInResult>();
            CreateMap<OWSignInCallEntity, OWSignInCall>();
            CreateMap<PartnerCallEntity, OWFailedSignInCall>();
            CreateMap<PartnerResultEntity, OWFailedSignInResult>();
            CreateMap<QueryLogRequest, QueryLogModel>().ForMember(request => request.LogType, opt => opt.MapFrom(src => (byte)src.LogType));
        }

        private static TRequest MapRequest<TRequest>(IRuntimeMapper mapper, ApiLogRequestEntity entity, Type sourceType, Type defaulDestinationType)
            where TRequest : IApiLogRequest
        {
            var destinationType = GetDestinationType(entity.RequestType) ?? defaulDestinationType;

            return destinationType != null ? (TRequest)mapper.Map(entity, sourceType, destinationType) : default;
        }

        private static TResult MapResult<TResult>(IRuntimeMapper mapper, ApiLogResultEntity entity, Type sourceType, Type defaulDestinationType)
            where TResult : IApiLogResult
        {
            var destinationType = GetDestinationType(entity.ResultType) ?? defaulDestinationType;

            return destinationType != null ? (TResult)mapper.Map(entity, sourceType, destinationType) : default;
        }

        private static IApiLogArchive MapArchive(IRuntimeMapper mapper, ApiLogArchiveEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var isBetArchive = entity.BetLogs != 0;
            var sourceType = typeof(ApiLogArchiveEntity);
            var destinationType = isBetArchive ? typeof(BetLogArchive) : typeof(ApiLogArchive);

            return (IApiLogArchive)mapper.Map(entity, sourceType, destinationType);
        }

        private static Type GetDestinationType(string typeName)
        {
            if (ApiLogConstant.CallOrRequestTypeMapping.ContainsKey(typeName))
            {
                return ApiLogConstant.CallOrRequestTypeMapping[typeName];
            }

            return null;
        }
    }
};