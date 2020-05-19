using System;

namespace GamesAdmin.Core.Models.Um
{
    public class UmSchedule
    {
        public UmSchedule(DateTime start, DateTime end, string message) 
        {
            Start = start;
            End = end;
            Message = message;
        }

        public DateTime Start { get;}

        public DateTime End { get; }

        public string Message { get; }
    }
}
