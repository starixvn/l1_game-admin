using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.ApiLogs;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.ApiLogs.Models;
using Refit;

namespace GamesAdmin.Site.Features
{
    public interface ILogApi : IBaseAuthorizationApi
    {
        [Post("/log/query")]
        Task<IEnumerable<dynamic>> Query(QueryLogModel model);
    }
}