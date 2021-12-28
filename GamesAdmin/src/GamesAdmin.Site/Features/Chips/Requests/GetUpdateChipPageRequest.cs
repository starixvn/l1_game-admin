using GamesAdmin.Site.Features.Chips.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Chips.Requests
{
    public class GetUpdateChipPageRequest : IRequest<UpdateChipViewModel>
    {
        public string Id { get; }

        public GetUpdateChipPageRequest(string id)
        {
            Id = id;
        }
    }
}