using MediatR;

namespace GamesAdmin.Api.Chips.Requests
{
    public class CreateChipModel
    {
        public string Text { get; set; }

        public double Value { get; set; }

        public string Color { get; set; }
    }

    public class CreateChipRequest : IRequest<bool>
    {
        public string Text { get; }

        public double Value { get; }

        public string Color { get; }

        public CreateChipRequest(string text, double value, string color)
        {
            Text = text;
            Value = value;
            Color = color;
        }
    }
}