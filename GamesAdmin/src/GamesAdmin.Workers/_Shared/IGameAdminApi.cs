using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using Refit;

namespace GamesAdmin.Workers._Shared
{
    public interface IGameAdminApi
    {
        [Post("/VietlottKeno/NotifyRoundResult")]
        Task NotifyRoundResult([Body] IEnumerable<VietlottKenoRound> kenoRounds);
    }
}