namespace GamesAdmin.Api.Chips.Requests
{
    public class UpdateChipModel : CreateChipModel
    {
        public string Id { get; set; }
    }

    public class UpdateChipRequest : CreateChipRequest
    {
        public UpdateChipRequest(string id, string text, int value) : base(text, value)
        {
            Id = id;
        }

        public string Id { get; }
    }
}