using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.ApiLogs.Requests;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models.ApiLogs;
using GamesAdmin.Database.ApiLogs.Entities;
using MediatR;

namespace GamesAdmin.Api.ApiLogs.Handlers
{
    public class QueryLogHandler : ApiLogHandler, IRequestHandler<QueryLogRequest, IEnumerable<IApiLog>>
    {
        public QueryLogHandler(IMapper mapper, IServiceProvider serviceProvider, IAppSettings appSettings) : base(mapper, serviceProvider, appSettings)
        {
        }

        public Task<IEnumerable<IApiLog>> Handle(QueryLogRequest request, CancellationToken cancellationToken)
            => IsSBLog(request.LogType) ? QuerySBLog(request) : QueryPartnerLog(request);

        private async Task<IEnumerable<IApiLog>> QuerySBLog(QueryLogRequest request)
        {
            var repository = GetRepository<SBLogEntity>(request.LogType);
            var dateRangeExpression = DateRangeExpression<SBLogEntity>(request);
            var gameTypeExpression = SBGameTypeExpression(request);
            var gameRoundExpression = SBGameRoundIdExpression(request);
            var obcustIdExpression = SBObCustIdExpression(request);
            var partnerIdExpession = SBPartnerIdExpression(request);
            var finalExpression = dateRangeExpression.And(gameTypeExpression)
                .And(gameRoundExpression)
                .And(obcustIdExpression)
                .And(partnerIdExpession);

            var entities = await repository.FilterByAsync(finalExpression);

            return Mapper.Map<IEnumerable<SBLog>>(entities);
        }

        private async Task<IEnumerable<IApiLog>> QueryPartnerLog(QueryLogRequest request)
        {
            var repository = GetRepository<PartnerLogEntity>(request.LogType);
            var dateRangeExpression = DateRangeExpression<PartnerLogEntity>(request);
            var gameTypeExpression = PartnerGameTypeExpression(request);
            var gameRoundExpression = PartnerGameRoundIdExpression(request);
            var obcustIdExpression = PartnerObCustIdExpression(request);
            var partnerIdExpession = PartnerIdExpression(request);
            var finalExpression = dateRangeExpression.And(gameTypeExpression)
                .And(gameRoundExpression)
                .And(obcustIdExpression)
                .And(partnerIdExpession);

            var entities = await repository.FilterByAsync(finalExpression);

            return Mapper.Map<IEnumerable<PartnerLog>>(entities);
        }

        private static Expression<Func<TEntity, bool>> DateRangeExpression<TEntity>(QueryLogRequest request)
            where TEntity : ApiLogEntity
            => entity => entity.LogTimeStamp >= request.FromDate && entity.LogTimeStamp < request.ToDate;

        private static Expression<Func<PartnerLogEntity, bool>> PartnerGameTypeExpression(QueryLogRequest request)
            => entity => request.GameTypeId == 0 || entity.Call.GameTypeId == 0 || request.GameTypeId == entity.Call.GameTypeId;

        private static Expression<Func<PartnerLogEntity, bool>> PartnerGameRoundIdExpression(QueryLogRequest request)
            => entity => request.GameRoundId == 0 || entity.Call.GameRoundId == 0 || request.GameRoundId == entity.Call.GameRoundId;

        private static Expression<Func<PartnerLogEntity, bool>> PartnerObCustIdExpression(QueryLogRequest request)
            => entity => request.ObCustId == 0 || entity.Call.ObCustId == 0 || request.ObCustId == entity.Call.ObCustId;

        private static Expression<Func<PartnerLogEntity, bool>> PartnerIdExpression(QueryLogRequest request)
            => entity => string.Equals(request.PartnerId, entity.PartnerId);

        private static Expression<Func<SBLogEntity, bool>> SBGameTypeExpression(QueryLogRequest request)
            => entity => request.GameTypeId == 0 || entity.Request.GameTypeId == 0 || request.GameTypeId == entity.Request.GameTypeId;

        private static Expression<Func<SBLogEntity, bool>> SBGameRoundIdExpression(QueryLogRequest request)
            => entity => request.GameRoundId == 0 || entity.Request.GameRoundId == 0 || request.GameRoundId == entity.Request.GameRoundId;

        private static Expression<Func<SBLogEntity, bool>> SBObCustIdExpression(QueryLogRequest request)
            => entity => request.ObCustId == 0 || entity.Request.ObCustId == 0 || request.ObCustId == entity.Request.ObCustId;

        private static Expression<Func<SBLogEntity, bool>> SBPartnerIdExpression(QueryLogRequest request)
            => entity => string.Equals(request.PartnerId, entity.Request.Auth.PartnerId);
    }
}