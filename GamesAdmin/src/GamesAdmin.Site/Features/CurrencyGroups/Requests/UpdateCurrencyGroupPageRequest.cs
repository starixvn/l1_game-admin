using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Requests
{
    public class UpdateCurrencyGroupPageRequest : IRequest<UpdateCurrencyGroupViewModel>
    {
        public string Id { get; set; }

        public UpdateCurrencyGroupPageRequest()
        {
        }

        public UpdateCurrencyGroupPageRequest(string id)
        {
            Id = id;
        }
    }
}