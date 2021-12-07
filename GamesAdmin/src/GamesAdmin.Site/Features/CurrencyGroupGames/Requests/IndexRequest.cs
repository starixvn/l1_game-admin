using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.Requests
{
    public class IndexRequest : IRequest<IndexViewModel>
    {
        public IndexRequest(string partnerId, string partnerName)
        {
            PartnerName = partnerName;
            PartnerId = partnerId;
        }

        public string PartnerId { get; }
        public string PartnerName { get; }
    }
}