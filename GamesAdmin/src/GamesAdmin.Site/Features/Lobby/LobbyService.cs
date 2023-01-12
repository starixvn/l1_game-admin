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

        Task<string> Update(LobbyModel lobbyModel);

        Task<CreateOrUpdateViewModel> GetLobbyConfig(string id);

        Task<bool> Delete(string id);
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
            var allLobbyConfigs = await this.lobbyApi.GetAll();
            var lobbyConfig = string.IsNullOrEmpty(id) ? null : await this.lobbyApi.Get(id);
            var otherLobbyConfigCurrencyGroups = allLobbyConfigs.Where(config => config.Id != lobbyConfig?.Id)
                .Select(config => config.CurrencyGroupIds).ToArray()
                .SelectMany(currencygroup => currencygroup).Distinct();

            var viewModel = new CreateOrUpdateViewModel
            {
                Partners = await partnerService.GetAll(),
                CurrencyGroupModels = (await this.currencyGroupApi.GetAll()).Where(currencyGroup => !otherLobbyConfigCurrencyGroups.Contains(currencyGroup.Id)),
                GameIds = Enumeration.GetAll<Game>()
                .Where(x => (byte)x.GameId != 0)
                .Select(x => new SelectListItem { Text = x.DisplayName, Value = ((int)x.GameId).ToString() })
                .ToList(),
                LobbyConfig = lobbyConfig
            };

            return viewModel;
        }

        public Task<string> Update(LobbyModel lobbyModel)
        {
            return this.lobbyApi.Update(lobbyModel);
        }

        public Task<bool> Delete(string id)
        {
            return this.lobbyApi.Delete(id);
        }
    }
}
