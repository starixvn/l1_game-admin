using System;
using AutoMapper;
using GamesAdmin.Api.ApiLogs.Requests;
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
                    model.LogType,
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
                    MapRequest<IApiLogRequest>(context, entity.Request, typeof(SBRequestEntity)),
                    entity.ResponseTimeStamp,
                    MapResult<ISBResult>(context, entity.Result, typeof(SBResultEntity))));

            CreateMap<PartnerLogEntity, PartnerLog>()
                .ConstructUsing((entity, context) => new PartnerLog(
                    entity.PartnerId,
                    entity.LogTimeStamp,
                    entity.RequestTimeStamp,
                    MapRequest<IApiLogRequest>(context, entity.Call, typeof(PartnerCallEntity)),
                    entity.ResponseTimeStamp,
                    MapResult<IPartnerResult>(context, entity.Result, typeof(PartnerResultEntity)),
                    entity.System,
                    entity.AccessKey));

            CreateMap<PartnerCallEntity, PartnerEndGameCall>();
            CreateMap<PartnerCallEntity, PartnerPlaceBetCall>();
            CreateMap<PartnerCallEntity, PartnerVoidGameCall>();

            CreateMap<PartnerResultEntity, PartnerResult>();
            CreateMap<PartnerResultEntity, PartnerPlaceBetResult>();

            CreateMap<SBRequestEntity, SignInRequest>();
            CreateMap<SBRequestEntity, EndGameInfoRequest>();

            CreateMap<SBResultEntity, SingleSignOnResult>();
            CreateMap<SBResultEntity, RetrieveEndGameInfoResult>();
        }

        private static TRequest MapRequest<TRequest>(IRuntimeMapper mapper, ApiLogRequestEntity entity, Type sourceType)
            where TRequest : IApiLogRequest
        {
            var destinationType = GetDestinationType(entity.RequestType);

            if (destinationType != null)
            {
                return (TRequest)mapper.Map(entity, sourceType, destinationType);
            }

            return default;
        }

        private static TResult MapResult<TResult>(IRuntimeMapper mapper, ApiLogResultEntity entity, Type sourceType)
            where TResult : IApiLogResult
        {
            var destinationType = GetDestinationType(entity.ResultType);

            if (destinationType != null)
            {
                return (TResult)mapper.Map(entity, sourceType, destinationType);
            }

            return default;
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
}