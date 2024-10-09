using GamesAdmin.Site.Features.GameGroupManagement.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameGroupManagement.Requests
{
    public class UpdatePageRequest : IRequest<UpdateGameGroupViewModel>
    {
        public UpdatePageRequest(string groupId)
        {
            Id = groupId;
        }

        public string Id { get; }
    }
}