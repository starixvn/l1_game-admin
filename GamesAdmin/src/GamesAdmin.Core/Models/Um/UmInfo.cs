using System;

namespace GamesAdmin.Core.Models.Um
{
    public class UmInfo
    {
        public UmInfo(DateTime start, DateTime end, bool finish, bool isPartnerUM, string partnerId) 
        {
            Start = start;
            End = end;
            Finish = finish;
            IsPartnerUM = isPartnerUM;
            PartnerId = partnerId;

        }
        public DateTimeOffset Start { get;}

        public DateTimeOffset End { get; }

        public bool Finish { get;}

        public bool IsPartnerUM { get;}

        public string PartnerId { get;}
    }
}
