using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.ExchangeRates.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.ExchangeRates
{
    public interface IExchangeRateService
    {
        Task<ExchangeRate> GetByExchangeId(int exchangeId);
    }

    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IMediator mediator;

        public ExchangeRateService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<ExchangeRate> GetByExchangeId(int exchangeId)
            => mediator.Send(new GetByExchangeIdRequest(exchangeId));
    }
}