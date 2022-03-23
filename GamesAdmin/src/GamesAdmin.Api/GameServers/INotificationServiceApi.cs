using System.Threading.Tasks;
using GamesAdmin.Api.UM.Requests;
using GamesAdmin.Core.Models;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface INotificationServiceApi
    {
        [Post("/api/um")]
        Task UnderMaintenance(ServerApiUMRequest request);

        [Post("/api/reload")]
        Task Reload();

        [Post("/api/proviet-keno/round-result")]
        Task NotifyRoundResult([Body] ProVietKenoRound kenoRound);
    }
}
