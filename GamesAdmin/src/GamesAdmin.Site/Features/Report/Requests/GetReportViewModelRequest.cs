using MediatR;
using GamesAdmin.Site.Features.Report.ViewModels;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetReportViewModelRequest : IRequest<ReportViewModel>
    {
        // No parameters needed for this request
    }
}
