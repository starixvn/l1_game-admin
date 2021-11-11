using System;
using AutoMapper;
using GamesAdmin.Api.ApiLogs.Requests;
using GamesAdmin.Core.Enumeration;
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
                    model.ObCustId,
                    model.GameRoundId,
                    model.GameTypeId));

            CreateMap<SBLogEntity, SBLog>()
                .ConstructUsing((entity, context) => new SBLog(
                    entity.LogTimeStamp,
                    entity.RequestTimeStamp,
                    MapRequest<IApiLogRequest>(context, entity.Request, typeof(SBRequestEntity), typeof(SBRequest)),
                    entity.ResponseTimeStamp,
                    MapResult<ISBResult>(context, entity.Result, typeof(SBResultEntity), typeof(SBResult))));

            CreateMap<PartnerLogEntity, PartnerLog>()
                .ConstructUsing((entity, context) => new PartnerLog(
                    entity.PartnerId,
                    entity.LogTimeStamp,
                    entity.RequestTimeStamp,
                    MapRequest<IApiLogRequest>(context, entity.Call, typeof(PartnerCallEntity), typeof(PartnerCall)),
                    entity.ResponseTimeStamp,
                    MapResult<IPartnerResult>(context, entity.Result, typeof(PartnerResultEntity), typeof(PartnerResult)),
                    entity.System,
                    entity.AccessKey));

            CreateMap<TicketDetailLogEntitty, TicketDetailLog>()
                .ConstructUsing((entity, context) => new TicketDetailLog(
                    entity.LogTimeStamp,
                    entity.RequestTimeStamp,
                    MapRequest<IApiLogRequest>(context, entity.Call, typeof(TicketDetailRequestEntity), typeof(TicketDetailRequest)),
                    entity.ResponseTimeStamp,
                    MapResult<ISBResult>(context, entity.Result, typeof(TicketDetailResultEntity), typeof(TicketDetailResult))));

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

            CreateMap<TicketDetailResultEntity, TicketDetailResult>();
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