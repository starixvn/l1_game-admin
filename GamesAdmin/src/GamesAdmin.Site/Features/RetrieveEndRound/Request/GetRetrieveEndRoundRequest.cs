namespace GamesAdmin.Site.Features.RetrieveEndRound.Request
{
    using GamesAdmin.Site.Features.BetDetails.ViewModels;
    using MediatR;

    public class GetRetrieveEndRoundRequest : IRequest<string>
    {
        public GetRetrieveEndRoundRequest(BetDetailsViewModel model)
        {
            Model = model;
        }

        public BetDetailsViewModel Model { get; }
    }
}
