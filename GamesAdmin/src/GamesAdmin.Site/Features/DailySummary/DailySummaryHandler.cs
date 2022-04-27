namespace GamesAdmin.Site.Features.DailySummary
{
    using System.Threading;
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.DailySummary.Requests;
    using GamesAdmin.Site.Features.DailySummary.ViewModels;
    using GamesAdmin.Site.Features.Partners;
    using MediatR;

    public class DailySummaryHandler : IRequestHandler<DailySummaryPageRequest, DailySummaryViewModel>, IRequestHandler<GetDailySummaryRequest, string>
    {
        private readonly IPartnerService operatorService;
        private readonly IDailySummaryService dailySummaryService;

        public DailySummaryHandler(IPartnerService operatorService, IDailySummaryService dailySummaryService)
        {
            this.operatorService = operatorService;
            this.dailySummaryService = dailySummaryService;
        }

        public async Task<DailySummaryViewModel> Handle(DailySummaryPageRequest request, CancellationToken cancellationToken)
        {
            var operators = await operatorService.GetAll();
            return new DailySummaryViewModel
            {
                Operators = operators,
            };
        }

        public async Task<string> Handle(GetDailySummaryRequest request, CancellationToken cancellationToken)
        {
            var Operator = await this.operatorService.GetById(request.Model.OperatorId);
            if (Operator != null)
            {
                request.Model.OperatorSecret = Operator.SecretKey;
            }
            return await dailySummaryService.GetDailySummarydAsync(request.Model);
        }
    }
}
