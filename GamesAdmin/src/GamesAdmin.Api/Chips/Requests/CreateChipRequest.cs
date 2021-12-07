using MediatR;

namespace GamesAdmin.Api.Chips.Requests
{
    public class CreateChipModel
    {
        public string Text { get; set; }

        public int Value { get; set; }

        public string Color { get; set; }
    }

    public class CreateChipRequest : IRequest<bool>
    {
        public string Text { get; }

        public int Value { get; }

        public string Color { get; }

        public CreateChipRequest(string text, int value, string color)
        {
            Text = text;
            Value = value;
            Color = color;
        }
    }
}