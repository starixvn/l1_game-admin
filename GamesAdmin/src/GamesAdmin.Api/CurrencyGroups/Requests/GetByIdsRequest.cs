using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroups.Requests
{
    public class GetByIdsModel
    {
        public IEnumerable<string> CurrencyGroups { get; set; }
    }

    public class GetByIdsRequest : IRequest<IEnumerable<CurrencyGroupModel>>
    {
        public GetByIdsModel Model { get; }

        public GetByIdsRequest(string ids)
        {
            Model = new GetByIdsModel
            {
                CurrencyGroups = ids.Split(",").ToList()
            };
        }
    }
}