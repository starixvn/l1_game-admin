using GamesAdmin.Site.Features._Shared;
using MediatR;

namespace GamesAdmin.Site.Features.Chips.Requests
{
    public class CreateChipRequest : IRequest<ResultBase>
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