using GamesAdmin.Core.Models;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Workers._Shared
{
    public interface IGameAdminApi
    {
        [Post("/ProVietKeno/NotifyRoundResult")]
        Task NotifyRoundResult([Body] IEnumerable<ProVietKenoRound> kenoRounds);
    }

    public interface IKenoWebApi 
    {
        [Get("/livekqxs/xstt/KN.php")]
        Task<string> GetResult();
    }
}