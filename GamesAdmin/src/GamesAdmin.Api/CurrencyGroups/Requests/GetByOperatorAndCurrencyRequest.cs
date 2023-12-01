using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroups.Requests
{
    public class GetByOperatorAndCurrencyRequest : IRequest<CurrencyGroupModel>
    {
        public string OperatorId { get; }
        public string CurrencyName { get; }

        public GetByOperatorAndCurrencyRequest(string operatorId, string currencyName)
        {
            OperatorId = operatorId;
            CurrencyName = currencyName;
        }
    }
}