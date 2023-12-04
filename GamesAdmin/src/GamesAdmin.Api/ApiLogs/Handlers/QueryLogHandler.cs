using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.ApiLogs.Apis;
using GamesAdmin.Api.ApiLogs.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.ApiLogs;
using GamesAdmin.Database.ApiLogs.Entities;
using MediatR;
using Sentry;

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
            if(AppSettings.CloudSettings?.ShouldCallCloudService(request.GameTypeId) == true)
            {
                return await GetLogRemotely(request);
            }

            return await GetLogDirectly(request, cancellationToken);
        }

        public async Task<IEnumerable<IApiLog>> GetLogRemotely(QueryLogRequest request)
        {
            var logs = Enumerable.Empty<IApiLog>();
            try
            {
                var logApi = (ILogApi)this.ServiceProvider.GetService(typeof(ILogApi));
                var remoteRequest = this.Mapper.Map<QueryLogModel>(request);
                logs = await logApi.Query(remoteRequest);
            }
            catch( Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }

            return logs; 
        }


        public Task<IEnumerable<IApiLog>> GetLogDirectly(QueryLogRequest request, CancellationToken cancellationToken)
            => request.LogType switch
            {
                ApiLogType.TicketDetailRequests => QueryTicketDetailLog(request),
                ApiLogType.RetrieveRequests => QuerySBLog(request),
                ApiLogType.SignInRequest => QuerySBLog(request),
                ApiLogType.MemberSiteSignInRequest => QueryMemberSignInLog(request),
                ApiLogType.OperatorSignInRequest => QueryOWSignInLog(request),
                _ => QueryPartnerLog(request)
            };

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