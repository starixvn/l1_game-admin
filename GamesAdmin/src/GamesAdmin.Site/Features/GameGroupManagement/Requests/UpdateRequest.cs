using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.GameGroupManagement.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameGroupManagement.Requests
{
    public class UpdateRequest : IRequest<MessageResult>
    {
        public UpdateRequest(UpdateGameGroupViewModel model) 
        {
            this.Model = model;
        }

        public UpdateGameGroupViewModel Model { get;}
    }
}