using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
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
            => entity => string.IsNullOrEmpty(request.PlayerId) || string.IsNullOrEmpty(entity.Call.PlayerId.ToString()) || request.PlayerId == entity.Call.PlayerId.ToString();

        private static Expression<Func<PartnerLogEntity, bool>> PartnerIdExpression(QueryLogRequest request)
            => entity => string.Equals(request.PartnerId, entity.PartnerId) || string.Equals(request.PartnerId, entity.OperatorId);

        private static Expression<Func<PartnerLogEntity, bool>> PartnerSiteIdExpression(QueryLogRequest request)
          => entity => request.SiteId == entity.Call.SiteId;

        private static Expression<Func<SBLogEntity, bool>> SBGameTypeExpression(QueryLogRequest request)
            => entity => request.GameTypeId == 0 || entity.Request.GameTypeId == 0 || request.GameTypeId == entity.Request.GameTypeId;

        private static Expression<Func<SBLogEntity, bool>> SBGameRoundIdExpression(QueryLogRequest request)
            => entity => request.GameRoundId == 0 || entity.Request.GameRoundId == 0 || request.GameRoundId == entity.Request.GameRoundId;

        private static Expression<Func<SBLogEntity, bool>> SBObCustIdExpression(QueryLogRequest request)
            => entity => string.IsNullOrEmpty(request.PlayerId) || string.IsNullOrEmpty(entity.Request.PlayerId.ToString()) || request.PlayerId == entity.Request.PlayerId.ToString();

        private static Expression<Func<SBLogEntity, bool>> SBPartnerIdExpression(QueryLogRequest request)
            => entity => string.Equals(request.PartnerId, entity.Request.Auth.OperatorId);

        private static Expression<Func<SBLogEntity, bool>> SBSiteIdExpression(QueryLogRequest request)
           => entity => request.SiteId == entity.Request.SiteId;

        private static Expression<Func<TicketDetailLogEntitty, bool>> TicketDetailGameTypeExpression(QueryLogRequest request)
            => entity => request.GameTypeId == 0 || entity.Call.GameTypeId == 0 || request.GameTypeId == entity.Call.GameTypeId;

        private static Expression<Func<TicketDetailLogEntitty, bool>> TicketDetailGameRoundIdExpression(QueryLogRequest request)
            => entity => request.GameRoundId == 0 || entity.Call.GameRoundId == 0 || request.GameRoundId == entity.Call.GameRoundId;

        private static Expression<Func<TicketDetailLogEntitty, bool>> TicketDetailObCustIdExpression(QueryLogRequest request)
            => entity => string.IsNullOrEmpty(request.PlayerId) || string.IsNullOrEmpty(entity.Call.PlayerId.ToString()) || request.PlayerId == entity.Call.PlayerId.ToString();

        private static Expression<Func<TicketDetailLogEntitty, bool>> TicketDetailPartnerIdExpression(QueryLogRequest request)
            => entity => string.Equals(request.PartnerId, entity.Call.Auth.OperatorId);

        private static Expression<Func<TicketDetailLogEntitty, bool>> TicketDetailSiteIdExpression(QueryLogRequest request)
           => entity => request.SiteId == entity.Call.SiteId;

        private static Expression<Func<MemberSignInLogEntity, bool>> UsernameExpression(QueryLogRequest request)
        {
            var regex = new Regex($"(?i){request.Username}");
            return entity => regex.IsMatch(entity.Request.Username) || regex.IsMatch(entity.Result.PublicUserInfo.UserName);
        }

        public static Expression<Func<PartnerLogEntity, bool>> GetPartnerExpression(QueryLogRequest request)
        {
            var dateRangeExpression = DateRangeExpression<PartnerLogEntity>(request);
            var gameTypeExpression = PartnerGameTypeExpression(request);
            var gameRoundExpression = PartnerGameRoundIdExpression(request);
            var obcustIdExpression = PartnerObCustIdExpression(request);
            var partnerIdExpession = PartnerIdExpression(request);
            var siteIdExpression = PartnerSiteIdExpression(request);

            var finalExpression = dateRangeExpression.And(gameTypeExpression)
                .And(gameRoundExpression)
                .And(obcustIdExpression)
                .And(partnerIdExpession)
                .And(!string.IsNullOrEmpty(request.SiteId) ? PartnerSiteIdExpression(request) : x => true);

            return finalExpression;
        }

        public static Expression<Func<SBLogEntity, bool>> GetSBExpression(QueryLogRequest request)
        {
            var dateRangeExpression = DateRangeExpression<SBLogEntity>(request);
            var gameTypeExpression = SBGameTypeExpression(request);
            var gameRoundExpression = SBGameRoundIdExpression(request);
            var obcustIdExpression = SBObCustIdExpression(request);
            var partnerIdExpession = SBPartnerIdExpression(request);
            var siteIdExpression = SBSiteIdExpression(request);
            var finalExpression = dateRangeExpression.And(gameTypeExpression)
                .And(gameRoundExpression)
                .And(obcustIdExpression)
                .And(partnerIdExpession)
                .And(!string.IsNullOrEmpty(request.SiteId) ? SBSiteIdExpression(request) : x => true);

            return finalExpression;
        }

        public static Expression<Func<TicketDetailLogEntitty, bool>> GetTicketDetailExpression(QueryLogRequest request)
        {
            var dateRangeExpression = DateRangeExpression<TicketDetailLogEntitty>(request);
            var gameTypeExpression = TicketDetailGameTypeExpression(request);
            var gameRoundExpression = TicketDetailGameRoundIdExpression(request);
            var obcustIdExpression = TicketDetailObCustIdExpression(request);
            var partnerIdExpession = TicketDetailPartnerIdExpression(request);
            var siteIdExpression =  TicketDetailSiteIdExpression(request);
            var finalExpression = dateRangeExpression.And(gameTypeExpression)
                .And(gameRoundExpression)
                .And(obcustIdExpression)
                .And(partnerIdExpession)
                .And(!string.IsNullOrEmpty(request.SiteId) ? TicketDetailSiteIdExpression(request) : x => true);

            return finalExpression;
        }

        public static Expression<Func<MemberSignInLogEntity, bool>> GetMemberSignInExpression(QueryLogRequest request)
        {
            var dateRangeExpression = DateRangeExpression<MemberSignInLogEntity>(request);
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                return dateRangeExpression;
            }

            return dateRangeExpression.And(UsernameExpression(request));
        }

        public static Expression<Func<OWSignInLogEntity, bool>> GetOWSignInExpression(QueryLogRequest request)
        {
            var expression = DateRangeExpression<OWSignInLogEntity>(request);
            expression = expression.And(x => string.IsNullOrEmpty(request.PlayerId) || request.PlayerId == x.Request.Call.ObCustId);
            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                var regex = new Regex($"(?i){request.Username}");
                return expression = expression.And(x => regex.IsMatch(x.Request.Call.ObCustName));
            }
            if (!string.IsNullOrWhiteSpace(request.SiteId))
            {
                return expression = expression.And(x => request.SiteId == x.Request.Call.SiteId);
            }

            return expression;
        }
    }
}