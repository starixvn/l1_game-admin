using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Site.Features.CurrencyGroups;
using GamesAdmin.Site.Features.Lobby.ViewModels;
using GamesAdmin.Site.Features.Partners;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Lobby
{
    public interface ILobbyService
    {
        Task<ViewModels.IndexViewModel> GetAll();

        Task<bool> Update(LobbyModel lobbyModel);

        Task<LobbyModel> Get(ObjectId Id);

        Task<CurrencyGroupViewModel> GetCurrencyGroups();
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

        public async Task<CurrencyGroupViewModel> GetCurrencyGroups()
        {
            var viewModel = new CurrencyGroupViewModel
            {
                Partners = await partnerService.GetAll(),
                CurrencyGroupModels = await this.currencyGroupApi.GetAll()
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
