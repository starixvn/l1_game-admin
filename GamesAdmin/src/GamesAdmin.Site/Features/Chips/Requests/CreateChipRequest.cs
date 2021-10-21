using GamesAdmin.Site.Features._Shared;
using MediatR;

namespace GamesAdmin.Site.Features.Chips.Requests
{
    public class CreateChipRequest : IRequest<ResultBase>
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