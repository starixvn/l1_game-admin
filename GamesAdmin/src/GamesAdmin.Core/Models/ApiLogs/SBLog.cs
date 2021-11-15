using System;

namespace GamesAdmin.Core.Models.ApiLogs
{
    public class SBLog : IApiLog
    {
        public SBLog(
            DateTimeOffset logTimeStamp,
            DateTimeOffset requestTimeStamp,
            IApiLogRequest request,
            DateTimeOffset responseTimeStamp,
            ISBResult result,
            IApiLogArchive archive)
        {
            LogTimeStamp = logTimeStamp;
            RequestTimeStamp = requestTimeStamp;
            Request = request;
            ResponseTimeStamp = responseTimeStamp;
            Result = result;
            Archive = archive;
        }

        public DateTimeOffset LogTimeStamp { get; }

        public DateTimeOffset RequestTimeStamp { get; }

        public IApiLogRequest Request { get; }

        public DateTimeOffset ResponseTimeStamp { get; }

        public ISBResult Result { get; }

        public IApiLogArchive Archive { get; }
    }
}