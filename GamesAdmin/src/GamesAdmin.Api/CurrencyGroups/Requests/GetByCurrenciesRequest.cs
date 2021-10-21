using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroups.Requests
{
    public class GetByCurrenciesAndPartnersModel
    {
        public IEnumerable<string> Currencies { get; set; }
    }

    public class GetByCurrenciesRequest : IRequest<IEnumerable<CurrencyGroupModel>>
    {
        public GetByCurrenciesRequest(GetByCurrenciesAndPartnersModel model)
        {
            Model = model;
        }

        public GetByCurrenciesAndPartnersModel Model { get; }
    }
}