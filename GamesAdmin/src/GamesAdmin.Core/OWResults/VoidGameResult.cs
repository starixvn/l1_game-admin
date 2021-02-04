using System;

namespace GamesAdmin.Core.OWResults
{
    public class VoidGameResult : OWResult
    {
        public int? ObCustId { get; set; }
    }

    public class VoidGameApiResult
    {
        public DateTimeOffset TimeStamp { get; set; }

        public VoidGameResult Result { get; set; }
    }
}