using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Site.Features.ExchangeRates.Requests
{
    public class GetByExchangeIdRequest : IRequest<ExchangeRate>
    {
        public GetByExchangeIdRequest(int exchangeId)
        {
            ExchangeId = exchangeId;
        }

        public int ExchangeId { get; }
    }
}