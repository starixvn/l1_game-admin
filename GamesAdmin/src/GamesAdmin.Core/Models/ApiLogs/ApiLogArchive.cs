using System;

namespace GamesAdmin.Core.Models.ApiLogs
{
    public class ApiLogArchive : IApiLogArchive
    {
        public DateTime EventTime { get; set; }
    }

    public class BetLogArchive : IApiLogArchive
    {
        public DateTime EventTime { get; set; }

        public int BetLogs { get; set; }
    }
}