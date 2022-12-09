using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Site.Features.CurrencyGroups;
using GamesAdmin.Site.Features.Lobby.ViewModels;
using GamesAdmin.Site.Features.Partners;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Lobby
{
    public interface ILobbyService
    {
        Task<ViewModels.IndexViewModel> GetAll();

        Task<bool> Update(LobbyModel lobbyModel);

        Task<LobbyModel> Get(ObjectId Id);

        Task<CreateOrUpdateViewModel> GetLobbyConfig(string id);
    }

    public class LobbyService : ILobbyService
    {
        private readonly ILobbyApi lobbyApi;
        private readonly ICurrencyGroupApi currencyGroupApi;
        private readonly IPartnerService partnerService;


        public LobbyService(ILobbyApi lobbyApi, ICurrencyGroupApi currencyGroupApi, IPartnerService partnerService)
        {
            this.lobbyApi = lobbyApi;
            this.currencyGroupApi = currencyGroupApi;
            this.partnerService = partnerService;
        }

        public async Task<LobbyModel> Get(ObjectId Id)
        {
            return null;
            //return this.lobbyApi.Get(Id);
        }

        public async Task<ViewModels.IndexViewModel> GetAll()
        {
            var currencyGroups = await currencyGroupApi.GetAll();
            var groupIds = currencyGroups.Select(group => group.Id).AsEnumerable();

            var viewModel = new ViewModels.IndexViewModel
            {
                LobbyConfigs = await this.lobbyApi.GetAll(),
                CurrencyGroupItems = new MultiSelectList(currencyGroups, "Id", "Name", groupIds),

            };

            return viewModel;
        }

        public async Task<CreateOrUpdateViewModel> GetLobbyConfig(string id)
        {
            var viewModel = new CreateOrUpdateViewModel
            {
                Partners = await partnerService.GetAll(),
                CurrencyGroupModels = await this.currencyGroupApi.GetAll(),
                GameIds = Enumeration.GetAll<Game>()
                .Where(x => (byte)x.GameId != 0)
                .Select(x => new SelectListItem { Text = x.DisplayName, Value = ((int)x.GameId).ToString() })
                .ToList()
            };

            return viewModel;
        }

        public Task<bool> Update(LobbyModel lobbyModel)
        {
            // return this.lobbyApi.Update(lobbyEntity);
            return this.lobbyApi.Update(lobbyModel);
        }
    }
}
