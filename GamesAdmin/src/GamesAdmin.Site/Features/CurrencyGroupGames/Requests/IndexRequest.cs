using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.Requests
{
    public class IndexRequest : IRequest<IndexViewModel>
    {
        public IndexRequest(string partnerId, string partnerName, string gameName)
        {
            PartnerName = partnerName;
            PartnerId = partnerId;
            GameName = gameName;
        }

        public string PartnerId { get; }
        public string PartnerName { get; }
        public string GameName { get; }
    }
}