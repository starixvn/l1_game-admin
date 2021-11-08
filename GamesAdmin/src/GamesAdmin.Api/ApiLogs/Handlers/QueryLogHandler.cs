using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.ApiLogs.Requests;
using GamesAdmin.Core.Models.ApiLogs;
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1067:Expressions should not be too complex", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1541:Methods and properties should not be too complex", Justification = "<Pending>")]
        private async Task<IEnumerable<IApiLog>> QuerySBLog(QueryLogRequest request)
        {
            var repository = GetSBRepository(request.LogType);
            var entities = await repository.FilterByAsync(entity
                => entity.LogTimeStamp >= request.FromDate
                   && entity.LogTimeStamp < request.ToDate
                   && (request.GameTypeId == 0 || entity.Request.GameTypeId == 0 || request.GameTypeId == entity.Request.GameTypeId)
                   && (request.GameRoundId == 0 || entity.Request.GameRoundId == 0 || request.GameRoundId == entity.Request.GameRoundId)
                   && (request.ObCustId == 0 || entity.Request.ObCustId == 0 || request.ObCustId == entity.Request.ObCustId)
                   && string.Equals(request.PartnerId, entity.Request.Auth.PartnerId));

            return Mapper.Map<IEnumerable<SBLog>>(entities);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1067:Expressions should not be too complex", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1541:Methods and properties should not be too complex", Justification = "<Pending>")]
        private async Task<IEnumerable<IApiLog>> QueryPartnerLog(QueryLogRequest request)
        {
            var repository = GetPartnerRepository(request.LogType);
            var entities = await repository.FilterByAsync(entity
                => entity.LogTimeStamp >= request.FromDate
                   && entity.LogTimeStamp < request.ToDate
                   && (request.GameTypeId == 0 || entity.Call.GameTypeId == 0 || request.GameTypeId == entity.Call.GameTypeId)
                   && (request.GameRoundId == 0 || entity.Call.GameRoundId == 0 || request.GameRoundId == entity.Call.GameRoundId)
                   && (request.ObCustId == 0 || entity.Call.ObCustId == 0 || request.ObCustId == entity.Call.ObCustId)
                   && string.Equals(request.PartnerId, entity.PartnerId));

            return Mapper.Map<IEnumerable<PartnerLog>>(entities);
        }
    }
}