using GamesAdmin.Site.Features.GameGroupManagement.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameGroupManagement.Requests
{
    public class CreatePageRequest : IRequest<CreateGameGroupViewModel>
    {
    }
}