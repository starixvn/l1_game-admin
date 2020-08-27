using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.DailySummary.Request;
using GamesAdmin.Core.Models.DailySummary;
using MediatR;

namespace GamesAdmin.Api.DailySummarize
{
    public class DailySummaryHandler : IRequestHandler<DailySummaryRequest, IEnumerable<DailySummaryResult>>
    {
        private IDailySummaryService service;

        public DailySummaryHandler(IDailySummaryService service)
        {
            this.service = service;
        }

        public Task<IEnumerable<DailySummaryResult>> Handle(DailySummaryRequest request, CancellationToken cancellationToken)
            => service.Get(request.SummarizedDate);
    }
}