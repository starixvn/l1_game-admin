using System.Collections.Generic;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class CreateAnnouncementsRequest : IRequest<bool>
    {
        public CreateAnnouncementsRequest(IEnumerable<CreateAnnouncementItemViewModel> models)
        {
            Models = models;
        }

        public IEnumerable<CreateAnnouncementItemViewModel> Models { get; }
    }
}
