using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.ApiLogs.Apis;
using GamesAdmin.Api.ApiLogs.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.IdentityServer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sentry;

namespace GamesAdmin.Api.ApiLogs
{
    [Route("api/log")]
    public class ApiLogController : BaseAuthorizeController
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IServiceProvider serviceProvider;
        private readonly IAppSettings appSettings;

        public ApiLogController(IMediator mediator, IMapper mapper, IAppSettings appSettings, IServiceProvider serviceProvider)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.appSettings = appSettings;
            this.serviceProvider = serviceProvider;
        }

        [HttpPost("query")]
        public async Task<IEnumerable<dynamic>> Query(QueryLogModel model)
        {
            var logs = Enumerable.Empty<dynamic>();

            if (ApiLogEnumeration.IsRelatedToGame((ApiLogType)model.LogType))
            {
                if (appSettings.CloudSettings?.ShouldCallCloudService(model.GameTypeId) == true)
                {
                    logs = logs.Concat(await GetLogRemotely(model));

                    return logs;
                }

                if (!appSettings.CloudSettings.IsCloudService && model.GameTypeId == 0)
                {
                    logs = logs.Concat(await GetLogRemotely(model));
                }
            }

            logs = logs.Concat(await mediator.Send(mapper.Map<QueryLogRequest>(model)));

            return logs;
        }

        private async Task<IEnumerable<object>> GetLogRemotely(QueryLogModel model)
        {
            try
            {
                var logApi = (ILogApi)serviceProvider.GetService(typeof(ILogApi));
                var header = await IdentityServerManager.GetAuthorizationHeader();

                var logs = await logApi.Query(model, header);

                return logs;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }

            return Enumerable.Empty<IEnumerable<object>>();
        }
    }
}