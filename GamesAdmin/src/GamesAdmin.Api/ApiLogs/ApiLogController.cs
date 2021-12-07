using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.ApiLogs.Requests;
using GamesAdmin.Core.Models.ApiLogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.ApiLogs
{
    [Route("api/log")]
    public class ApiLogController : BaseAuthorizeController
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ApiLogController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpPost("query")]
        public Task<IEnumerable<IApiLog>> Query(QueryLogModel model)
            => mediator.Send(mapper.Map<QueryLogRequest>(model));
    }
}