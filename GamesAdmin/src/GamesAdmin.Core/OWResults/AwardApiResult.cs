using System;

namespace GamesAdmin.Core.OWResults
{
    public class AwardApiResult
    {
        public DateTimeOffset TimeStamp { get; set; }

        public OWResult Result { get; set; }
    }
}
