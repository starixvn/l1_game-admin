using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.CurrencyGroupGames;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;
using GamesAdmin.Site.Features.Partners;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Handlers
{
    public class QueryCurrencyGroupsHandler : Handler, IRequestHandler<QueryCurrencyGroupsRequest, CurrencyGroupManagementViewModel>
    {
        private readonly IPartnerService partnerService;
        private readonly ICurrencyGroupGameService gameService;

        public QueryCurrencyGroupsHandler(IMapper mapper, ICurrencyGroupApi api, IPartnerService partnerService, ICurrencyGroupGameService gameService) : base(mapper, api)
        {
            this.partnerService = partnerService;
            this.gameService = gameService;
        }

        public async Task<CurrencyGroupManagementViewModel> Handle(QueryCurrencyGroupsRequest request, CancellationToken cancellationToken)
        {
            var partners = await partnerService.GetPartnersForSelection(partner => true);

            if (request.Model.CurrencyGroups == null || !request.Model.CurrencyGroups.Any())
            {
                return new CurrencyGroupManagementViewModel
                {
                    Models = new List<CurrencyGroupViewModel>(),
                    PartnerItems = partners,
                };
            }

            var games = await gameService.GetAll();
            var groups = await Api.Query(request.Model);
            var groupsWithGames = groups.Select(group =>
            {
                var viewModel = Mapper.Map<CurrencyGroupViewModel>(group);

                viewModel.Games = GetGames(games, group.Id);

                return viewModel;
            });

            return new CurrencyGroupManagementViewModel
            {
                Models = groupsWithGames,
                PartnerItems = partners,
            };
        }

        private static IEnumerable<string> GetGames(IEnumerable<CurrencyGroupGame> games, string groupId)
        {
            return games
                .Where(game => string.Equals(game.GroupId, groupId))
                .Select(game => game.GameName)
                .AsEnumerable();
        }
    }
}