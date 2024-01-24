using System;

namespace GamesAdmin.Core.Models.ApiLogs
{
    public interface IApiLog
    {
        DateTimeOffset LogTimeStamp { get; }

        DateTimeOffset RequestTimeStamp { get; }

        DateTimeOffset ResponseTimeStamp { get; }

        IApiLogArchive Archive { get; }
    }

    public interface IApiLogRequest
    {
        string Seq { get; }

        int? PlayerId { get; }

        string SiteId { get; }

        int? GameRoundId { get; }

        int? GameTypeId { get; }

        string RequestType { get; }
    }

    public interface IApiLogResult
    {
        string ResultType { get; }
    }

    public interface IApiLogArchive
    {
        DateTime EventTime { get; }
    }
}