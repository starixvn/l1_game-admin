using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroups.Requests
{
    public class CreateCurrencyGroupModel
    {
        public string Name { get; set; }

        public string PartnerId { get; set; }

        public bool Enabled { get; set; }

        public IEnumerable<string> Currencies { get; set; }
    }

    public class CreateRequest : IRequest<bool>
    {
        public CreateRequest(CreateCurrencyGroupModel model)
        {
            Model = model;
        }

        public CreateCurrencyGroupModel Model { get; }
    }
}