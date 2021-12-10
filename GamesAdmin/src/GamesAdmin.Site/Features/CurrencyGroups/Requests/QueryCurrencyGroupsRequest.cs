using GamesAdmin.Site.Features.CurrencyGroups.Models;
using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Requests
{
    public class QueryCurrencyGroupsRequest : IRequest<CurrencyGroupManagementViewModel>
    {
        public QueryCurrencyGroupsRequest(QueryCurrencyGroupsModel model)
        {
            Model = model;
        }

        public QueryCurrencyGroupsModel Model { get; }
    }
}