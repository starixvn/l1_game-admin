using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Partners;
using GamesAdmin.Site.Features.Report.ViewModels;

namespace GamesAdmin.Site.Features.Report
{
    public interface IGameReportService
    {
        Task<ReportResultViewModel> Get(BetReportFilter filter);
    }

    public abstract class GameReportService : IGameReportService
    {
        protected readonly IReportApi reportApi;
        protected readonly IPartnerService partnerService;
        protected readonly IMapper mapper;
        protected readonly IAppSettings appSettings;

        protected GameReportService(IReportApi reportApi, IPartnerService partnerService, IMapper mapper, IAppSettings appSettings)
        {
            this.reportApi = reportApi;
            this.partnerService = partnerService;
            this.mapper = mapper;
            this.appSettings = appSettings;
        }

        public abstract Task<ReportResultViewModel> Get(BetReportFilter filter);

        protected static string GetPartnerName(IEnumerable<Partner> partners, string partnerId)
         => partners.FirstOrDefault(p => string.Equals(p.PartnerId, partnerId))?.Name ?? string.Empty;
    }
}