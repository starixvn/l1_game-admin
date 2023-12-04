using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api.ApiLogs.Requests;
using GamesAdmin.Core.Models.ApiLogs;
using Refit;

namespace GamesAdmin.Api.ApiLogs.Apis
{
    [Headers("Authorization: Bearer")]
    public interface ILogApi
    {
        [Post("/api/log/query")]
        Task<IEnumerable<IApiLog>> Query([Body] QueryLogModel query);
    }
}