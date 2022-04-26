namespace GamesAdmin.Site.Features.BetDetails.Requests
{
    using GamesAdmin.Site.Features.BetDetails.ViewModels;
    using MediatR;

    public class GetUrlAccessBetDetailsRequest : IRequest<string>
    {
        public GetUrlAccessBetDetailsRequest(BetDetailsViewModel model)
        {
            Model = model;
        }

        public BetDetailsViewModel Model { get; }
    }
}
