using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.ApiLogs.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.ApiLogs;
using GamesAdmin.Database.ApiLogs.Entities;
using MediatR;

namespace GamesAdmin.Api.ApiLogs.Handlers
{
    public class QueryLogHandler : ApiLogHandler, IRequestHandler<QueryLogRequest, IEnumerable<IApiLog>>
    {
        public QueryLogHandler(IMapper mapper, IServiceProvider serviceProvider, IAppSettings appSettings)
            : base(mapper, serviceProvider, appSettings)
        {
        }

        public async Task<IEnumerable<IApiLog>> Handle(QueryLogRequest request, CancellationToken cancellationToken)
        {
            var logs = await (request.LogType switch
            {
                ApiLogType.TicketDetailRequests => QueryTicketDetailLog(request),
                ApiLogType.RetrieveRequests => QuerySBLog(request),
                ApiLogType.SignInRequest => QuerySBLog(request),
                ApiLogType.MemberSiteSignInRequest => QueryMemberSignInLog(request),
                ApiLogType.OperatorSignInRequest => QueryOWSignInLog(request),
                _ => QueryPartnerLog(request)
            });

            return logs;
        }

        private async Task<IEnumerable<IApiLog>> QuerySBLog(QueryLogRequest request)
        {
            var repository = GetRepository<SBLogEntity>(request.LogType);
            var expression = ApiLogQueryExpression.GetSBExpression(request);

            var entities = await repository.FilterByAsync(expression);

            return Mapper.Map<IEnumerable<SBLog>>(entities.AsEnumerable());
        }

        private async Task<IEnumerable<IApiLog>> QueryPartnerLog(QueryLogRequest request)
        {
            var repository = GetRepository<PartnerLogEntity>(request.LogType);
            var expression = ApiLogQueryExpression.GetPartnerExpression(request);

            var entities = await repository.FilterByAsync(expression);

            return Mapper.Map<IEnumerable<PartnerLog>>(entities.AsEnumerable());
        }

        private async Task<IEnumerable<IApiLog>> QueryMemberSignInLog(QueryLogRequest request)
        {
            var repository = GetRepository<MemberSignInLogEntity>(request.LogType);
            var expression = ApiLogQueryExpression.GetMemberSignInExpression(request);

            var entities = await repository.FilterByAsync(expression);

            return Mapper.Map<IEnumerable<MemberSignInLog>>(entities.AsEnumerable());
        }

        private async Task<IEnumerable<IApiLog>> QueryOWSignInLog(QueryLogRequest request)
        {
            var repository = GetRepository<OWSignInLogEntity>(request.LogType);
            var expression = ApiLogQueryExpression.GetOWSignInExpression(request);

            var entities = await repository.FilterByAsync(expression);

            return Mapper.Map<IEnumerable<OWSignInLog>>(entities.AsEnumerable());
        }

        private async Task<IEnumerable<IApiLog>> QueryTicketDetailLog(QueryLogRequest request)
        {
            var repository = GetRepository<TicketDetailLogEntitty>(request.LogType);
            var expression = ApiLogQueryExpression.GetTicketDetailExpression(request);

            var entities = await repository.FilterByAsync(expression);

            return Mapper.Map<IEnumerable<TicketDetailLog>>(entities.AsEnumerable());
        }
    }
}