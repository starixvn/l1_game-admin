using System;
using System.Linq.Expressions;
using GamesAdmin.Api.ApiLogs.Requests;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Database.ApiLogs.Entities;

namespace GamesAdmin.Api.ApiLogs
{
    public static class ApiLogQueryExpression
    {
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

        private static Expression<Func<TicketDetailLogEntitty, bool>> TicketDetailGameTypeExpression(QueryLogRequest request)
            => entity => request.GameTypeId == 0 || entity.Call.GameTypeId == 0 || request.GameTypeId == entity.Call.GameTypeId;

        private static Expression<Func<TicketDetailLogEntitty, bool>> TicketDetailGameRoundIdExpression(QueryLogRequest request)
            => entity => request.GameRoundId == 0 || entity.Call.GameRoundId == 0 || request.GameRoundId == entity.Call.GameRoundId;

        private static Expression<Func<TicketDetailLogEntitty, bool>> TicketDetailObCustIdExpression(QueryLogRequest request)
            => entity => request.ObCustId == 0 || entity.Call.ObCustId == 0 || request.ObCustId == entity.Call.ObCustId;

        private static Expression<Func<TicketDetailLogEntitty, bool>> TicketDetailPartnerIdExpression(QueryLogRequest request)
            => entity => string.Equals(request.PartnerId, entity.Call.Auth.PartnerId);

        public static Expression<Func<PartnerLogEntity, bool>> GetPartnerExpression(QueryLogRequest request)
        {
            var dateRangeExpression = DateRangeExpression<PartnerLogEntity>(request);
            var gameTypeExpression = PartnerGameTypeExpression(request);
            var gameRoundExpression = PartnerGameRoundIdExpression(request);
            var obcustIdExpression = PartnerObCustIdExpression(request);
            var partnerIdExpession = PartnerIdExpression(request);
            var finalExpression = dateRangeExpression.And(gameTypeExpression)
                .And(gameRoundExpression)
                .And(obcustIdExpression)
                .And(partnerIdExpession);

            return finalExpression;
        }

        public static Expression<Func<SBLogEntity, bool>> GetSBExpression(QueryLogRequest request)
        {
            var dateRangeExpression = DateRangeExpression<SBLogEntity>(request);
            var gameTypeExpression = SBGameTypeExpression(request);
            var gameRoundExpression = SBGameRoundIdExpression(request);
            var obcustIdExpression = SBObCustIdExpression(request);
            var partnerIdExpession = SBPartnerIdExpression(request);
            var finalExpression = dateRangeExpression.And(gameTypeExpression)
                .And(gameRoundExpression)
                .And(obcustIdExpression)
                .And(partnerIdExpession);

            return finalExpression;
        }

        public static Expression<Func<TicketDetailLogEntitty, bool>> GetTicketDetailExpression(QueryLogRequest request)
        {
            var dateRangeExpression = DateRangeExpression<TicketDetailLogEntitty>(request);
            var gameTypeExpression = TicketDetailGameTypeExpression(request);
            var gameRoundExpression = TicketDetailGameRoundIdExpression(request);
            var obcustIdExpression = TicketDetailObCustIdExpression(request);
            var partnerIdExpession = TicketDetailPartnerIdExpression(request);
            var finalExpression = dateRangeExpression.And(gameTypeExpression)
                .And(gameRoundExpression)
                .And(obcustIdExpression)
                .And(partnerIdExpession);

            return finalExpression;
        }
    }
}