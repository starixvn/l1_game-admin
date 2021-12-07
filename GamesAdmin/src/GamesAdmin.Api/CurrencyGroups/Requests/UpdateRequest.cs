using MediatR;

namespace GamesAdmin.Api.CurrencyGroups.Requests
{
    public class UpdateCurrencyGroupModel : CreateCurrencyGroupModel
    {
        public string Id { get; set; }
    }

    public class UpdateRequest : IRequest<bool>
    {
        public UpdateRequest(UpdateCurrencyGroupModel model)
        {
            Model = model;
        }

        public UpdateCurrencyGroupModel Model { get; }
    }
}