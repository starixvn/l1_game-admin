using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroups.Requests
{
    public class GetByIdRequest : IRequest<CurrencyGroupModel>
    {
        public string Id { get; }

        public GetByIdRequest(string id)
        {
            Id = id;
        }
    }
}