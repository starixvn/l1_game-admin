using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;

namespace GamesAdmin.Api.Currencies.Requests
{
    public class GetByGroupModel
    {
        public string GroupId { get; set; }

        public GetByGroupModel(string groupId)
        {
            GroupId = groupId;
        }
    }

    public class GetByGroupRequest : Request<GetByGroupModel, IEnumerable<CurrencyModel>>
    {
        public GetByGroupRequest(GetByGroupModel model) : base(model)
        {
        }
    }
}