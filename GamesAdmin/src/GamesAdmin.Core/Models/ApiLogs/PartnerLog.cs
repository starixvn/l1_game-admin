using System;

namespace GamesAdmin.Core.Models.ApiLogs
{
    public class PartnerLog : IApiLog
    {
#pragma warning disable S107 // Methods should not have too many parameters

        public PartnerLog(
            string partnerId,
            DateTimeOffset logTimeStamp,
            DateTimeOffset requestTimeStamp,
            IApiLogRequest call,
            DateTimeOffset responseTimeStamp,
            IPartnerResult result,
            byte system,
            string accessKey)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            PartnerId = partnerId;
            LogTimeStamp = logTimeStamp;
            RequestTimeStamp = requestTimeStamp;
            Call = call;
            ResponseTimeStamp = responseTimeStamp;
            Result = result;
            System = system;
            AccessKey = accessKey;
        }

        public string PartnerId { get; }

        public DateTimeOffset LogTimeStamp { get; }

        public DateTimeOffset RequestTimeStamp { get; }

        public IApiLogRequest Call { get; }

        public DateTimeOffset ResponseTimeStamp { get; }

        public IPartnerResult Result { get; }

        public byte System { get; }

        public string AccessKey { get; }
    }
}