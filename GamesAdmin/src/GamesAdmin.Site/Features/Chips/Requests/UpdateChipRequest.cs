namespace GamesAdmin.Site.Features.Chips.Requests
{
    public class UpdateChipRequest : CreateChipRequest
    {
        public UpdateChipRequest(string id, string text, int value) : base(text, value)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}