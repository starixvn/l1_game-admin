using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api.ApiLogs.Requests;
using Refit;

namespace GamesAdmin.Api.ApiLogs.Apis
{
    public interface ILogApi
    {
        [Post("/api/log/query")]
        Task<IEnumerable<dynamic>> Query([Body] QueryLogModel query, [Header("Authorization")] string header = "");
    }
}