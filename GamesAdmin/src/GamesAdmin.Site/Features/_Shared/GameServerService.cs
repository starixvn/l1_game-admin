using System.Threading.Tasks;
using Refit;

namespace GamesAdmin.Site.Features._Shared
{
    public interface IGameServerApi 
    {
        [Post("/reload")]
        Task Reload();
    }

    public interface IGameServerService 
    {
        Task Reload();
    }
    public class GameServerService : IGameServerService
    {
        private readonly IGameServerApi serverApi;

        public GameServerService(IGameServerApi serverApi)
        {
            this.serverApi = serverApi;   
        }

        public Task Reload() => serverApi.Reload();
    }
}
