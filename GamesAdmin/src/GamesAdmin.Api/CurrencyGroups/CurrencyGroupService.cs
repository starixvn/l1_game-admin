using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api.CurrencyGroups.Requests;
using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroups
{
    public interface ICurrencyGroupService
    {
        Task<IEnumerable<CurrencyGroupModel>> GetAll();

        Task<CurrencyGroupModel> GetById(string id);

        Task<CurrencyGroupModel> GetByOperatorAndCurrency(string operatorId, string currencyName);
    }

    public class CurrencyGroupService : ICurrencyGroupService
    {
        private readonly IMediator mediator;

        public CurrencyGroupService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<IEnumerable<CurrencyGroupModel>> GetAll()
            => mediator.Send(new GetAllRequest());

        public Task<CurrencyGroupModel> GetById(string id)
            => mediator.Send(new GetByIdRequest(id));

        public Task<CurrencyGroupModel> GetByOperatorAndCurrency(string operatorId, string currencyName)
            => mediator.Send(new GetByOperatorAndCurrencyRequest(operatorId, currencyName));

        public Task<IEnumerable<CurrencyGroupModel>> GetByPartner(string partnerId)
            => mediator.Send(new GetByPartnerRequest(partnerId));
    }
}