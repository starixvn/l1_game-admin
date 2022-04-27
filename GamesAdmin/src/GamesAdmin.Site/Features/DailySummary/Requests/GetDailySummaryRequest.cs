namespace GamesAdmin.Site.Features.DailySummary.Requests
{
    using GamesAdmin.Site.Features.DailySummary.ViewModels;
    using MediatR;

    public class GetDailySummaryRequest : IRequest<string>
    {
        public GetDailySummaryRequest(DailySummaryViewModel model)
        {
            Model = model;
        }

        public DailySummaryViewModel Model { get; }
    }
}
