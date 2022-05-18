using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.ApiLogs
{
    public class PartnerLog : IApiLog
    {
#pragma warning disable S107 // Methods should not have too many parameters

        public PartnerLog(
            string partnerId,
            string operatorId,
            DateTimeOffset logTimeStamp,
            DateTimeOffset requestTimeStamp,
            IApiLogRequest call,
            DateTimeOffset responseTimeStamp,
            IPartnerResult result,
            byte system,
            string accessKey,
            IApiLogArchive archive)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            PartnerId = partnerId;
            OperatorId = operatorId;
            LogTimeStamp = logTimeStamp;
            RequestTimeStamp = requestTimeStamp;
            Call = call;
            ResponseTimeStamp = responseTimeStamp;
            Result = result;
            System = system;
            AccessKey = accessKey;
            Archive = archive;
        }

        public string PartnerId { get; }
        public string OperatorId { get; }

        public DateTimeOffset LogTimeStamp { get; }

        public DateTimeOffset RequestTimeStamp { get; }

        public IApiLogRequest Call { get; }

        public DateTimeOffset ResponseTimeStamp { get; }

        public IPartnerResult Result { get; }

        public byte System { get; }

        public string AccessKey { get; }

        public IApiLogArchive Archive { get; }

        public Dictionary<string, string> Metadata { get; set; }
    }
}