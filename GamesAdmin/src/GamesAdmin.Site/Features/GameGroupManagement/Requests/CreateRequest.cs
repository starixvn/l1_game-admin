using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.GameGroupManagement.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameGroupManagement.Requests
{
    public class CreateRequest : IRequest<MessageResult>
    {
        public CreateRequest(CreateGameGroupViewModel model) 
        {
            this.Model = model;
        }

        public CreateGameGroupViewModel Model { get;}
    }
}