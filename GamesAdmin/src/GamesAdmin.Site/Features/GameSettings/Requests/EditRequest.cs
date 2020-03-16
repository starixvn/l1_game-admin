using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class EditRequest : IRequest<EditViewModel>
    {
        public EditRequest(string id) 
        {
            Id = id;
        }

        public string Id { get; }
    }
}
