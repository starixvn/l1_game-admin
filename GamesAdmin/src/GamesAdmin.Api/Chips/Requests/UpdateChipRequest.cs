namespace GamesAdmin.Api.Chips.Requests
{
    public class UpdateChipModel : CreateChipModel
    {
        public string Id { get; set; }
    }

    public class UpdateChipRequest : CreateChipRequest
    {
        public UpdateChipRequest(string id, string text, double value, string color) : base(text, value, color)
        {
            Id = id;
        }

        public string Id { get; }
    }
}