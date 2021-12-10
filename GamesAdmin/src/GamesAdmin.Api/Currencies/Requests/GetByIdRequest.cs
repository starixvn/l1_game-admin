using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Api.Currencies.Requests
{
    public class GetByIdRequest : IRequest<CurrencyModel>
    {
        public string Id { get; }

        public GetByIdRequest(string id)
        {
            Id = id;
        }
    }
}