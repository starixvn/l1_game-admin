using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroups.Requests
{
    public class GetByCurrenciesAndPartnersModel
    {
        public IEnumerable<string> Currencies { get; set; }

        public IEnumerable<string> Partners { get; set; }
    }

    public class GetByCurrenciesAndPartnersRequest : IRequest<IEnumerable<CurrencyGroupModel>>
    {
        public GetByCurrenciesAndPartnersRequest(GetByCurrenciesAndPartnersModel model)
        {
            Model = model;
        }

        public GetByCurrenciesAndPartnersModel Model { get; }
    }
}