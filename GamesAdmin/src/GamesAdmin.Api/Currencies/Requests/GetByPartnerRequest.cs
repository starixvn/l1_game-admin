using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;

namespace GamesAdmin.Api.Currencies.Requests
{
    public class GetByPartnerModel
    {
        public string PartnerId { get; set; }

        public GetByPartnerModel() { }

        public GetByPartnerModel(string partnerId)
        {
            PartnerId = partnerId;
        }
    }

    public class GetByPartnerRequest : Request<GetByPartnerModel, IEnumerable<CurrencyModel>>
    {
        public GetByPartnerRequest(GetByPartnerModel model) : base(model)
        {
        }
    }
}