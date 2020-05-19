using System;

namespace GamesAdmin.Core.Models.Um
{
    public class UmSchedule
    {
        public UmSchedule(DateTime start, DateTime end, string message = "") 
        {
            Start = start;
            End = end;
            Message = message;
        }

        public DateTimeOffset Start { get;}

        public DateTimeOffset End { get; }

        public string Message { get; }
    }
}
