namespace GamesAdmin.Api.Currencies.Requests
{
    public class CreateCurrencyModel
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public double Rate { get; set; }

        public bool Enabled { get; set; }

        public bool IsTest { get; set; }

        public string PartnerId { get; set; }

        public string CurrencyGroupId { get; set; }
    }

    public class CreateRequest : Request<CreateCurrencyModel, bool>
    {
        public CreateRequest(CreateCurrencyModel model) : base(model)
        {
        }
    }
}