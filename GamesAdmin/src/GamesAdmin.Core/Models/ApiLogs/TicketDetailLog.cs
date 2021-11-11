using System;

namespace GamesAdmin.Core.Models.ApiLogs
{
    public class TicketDetailLog : IApiLog
    {
        public TicketDetailLog(DateTimeOffset logTimeStamp, DateTimeOffset requestTimeStamp, IApiLogRequest call, DateTimeOffset responseTimeStamp, ISBResult result)
        {
            LogTimeStamp = logTimeStamp;
            RequestTimeStamp = requestTimeStamp;
            Call = call;
            ResponseTimeStamp = responseTimeStamp;
            Result = result;
        }

        public DateTimeOffset LogTimeStamp { get; }

        public DateTimeOffset RequestTimeStamp { get; }

        public IApiLogRequest Call { get; }

        public DateTimeOffset ResponseTimeStamp { get; }

        public ISBResult Result { get; }
    }
}