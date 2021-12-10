namespace GamesAdmin.Api.Currencies.Requests
{
    public class UpdateCurrencyModel
    {
        public string Id { get; set; }

        public bool Enabled { get; set; }

        public bool IsTest { get; set; }

        public string GroupId { get; set; }
    }

    public class UpdateRequest : Request<UpdateCurrencyModel, bool>
    {
        public UpdateRequest(UpdateCurrencyModel model) : base(model)
        {
        }
    }
}