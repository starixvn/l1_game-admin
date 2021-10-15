namespace GamesAdmin.Api.Currencies.Requests
{
    public class UpdateCurrencyModel : CreateCurrencyModel
    {
        public string Id { get; set; }
    }

    public class UpdateRequest : Request<UpdateCurrencyModel, bool>
    {
        public UpdateRequest(UpdateCurrencyModel model) : base(model)
        {
        }
    }
}