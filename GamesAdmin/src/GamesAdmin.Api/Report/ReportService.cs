using System.Collections.Generic;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public interface IReportService
    {
        IEnumerable<BigSmallReportEntity> GetBigSmallReport(long roundId, string nickname, bool excludeBot);
    }

    public class ReportService : IReportService
    {
        private readonly IBigSmallReportRepository bigSmallReportRepository;

        public ReportService(IBigSmallReportRepository bigSmallReportRepository)
        {
            this.bigSmallReportRepository = bigSmallReportRepository;
        }

        public IEnumerable<BigSmallReportEntity> GetBigSmallReport(long roundId, string nickname, bool excludeBot) => bigSmallReportRepository.Get(roundId, nickname, excludeBot);
    }
}