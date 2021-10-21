using MediatR;

namespace GamesAdmin.Api.Chips.Requests
{
    public class CreateChipModel
    {
        public string Text { get; set; }

        public int Value { get; set; }
    }

    public class CreateChipRequest : IRequest<bool>
    {
        public string Text { get; }

        public int Value { get; }

        public CreateChipRequest(string text, int value)
        {
            Text = text;
            Value = value;
        }
    }
}