using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetSelectionsRequest : IRequest<ReportSeletionsViewModel>
    {
    }
}