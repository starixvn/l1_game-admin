using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.ApiLogs.Models;
using GamesAdmin.Site.Features.ApiLogs.Requests;
using GamesAdmin.Site.Features.ApiLogs.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ApiLogs.Handlers
{
    public class QueryApiLogHandler : ApiLogHandler, IRequestHandler<QueryApiLogRequest, ApiLogListViewModel>
    {
        public QueryApiLogHandler(ILogApi api, IMapper mapper) : base(api, mapper)
        {
        }

        public async Task<ApiLogListViewModel> Handle(QueryApiLogRequest request, CancellationToken cancellationToken)
        {
            request.ToDate += new TimeSpan(23, 59, 59);
            var model = Mapper.Map<QueryLogModel>(request);
            var logs = await Api.Query(model);
            var configs = ApiLogConstant.ViewConfigsMappings[(ApiLogType)request.LogType];

            return new ApiLogListViewModel
            {
                Logs = logs,
                Configs = configs
            };
        }
    }
}