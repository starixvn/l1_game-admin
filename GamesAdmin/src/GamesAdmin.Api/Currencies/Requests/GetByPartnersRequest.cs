using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;

namespace GamesAdmin.Api.Currencies.Requests
{
    public class GetByPartnersModel
    {
        public IEnumerable<string> PartnerIds { get; set; }

        public GetByPartnersModel(IEnumerable<string> partnerIds)
        {
            PartnerIds = partnerIds;
        }
    }

    public class GetByPartnersRequest : Request<GetByPartnersModel, IEnumerable<CurrencyModel>>
    {
        public GetByPartnersRequest(GetByPartnersModel model) : base(model)
        {
        }
    }
}