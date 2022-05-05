using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.ExchangeRates.Requests
{
    public class GetByExchangeIdRequest : IRequest<ExchangeRate>
    {
        public GetByExchangeIdRequest(int exId)
        {
            ExchangeId = exId;
        }

        public int ExchangeId { get; }
    }
}