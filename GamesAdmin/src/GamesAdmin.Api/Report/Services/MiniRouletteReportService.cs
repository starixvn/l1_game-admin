using AutoMapper;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public class MiniRouletteReportService : RouletteReportService
    {
        public MiniRouletteReportService(IRouletteReportRepository repository, IBetHistoryReportRepository betHistoryRepository, IMapper mapper) : base(repository, betHistoryRepository, mapper)
        {
        }
    }
}
