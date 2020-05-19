using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.UmSchedule.ViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel(IList<UmScheduleItemViewModel> schedules)
        {
            Schedules = schedules;
        }

        public IList<UmScheduleItemViewModel> Schedules { get; }
    }

    public class UmScheduleItemViewModel
    {
        public UmScheduleItemViewModel(
            DateTimeOffset start,
            DateTimeOffset end, 
            string message = "")
        {
            Start = start;
            End = end;
            Message = message;
        }

        public DateTimeOffset Start { get; }

        public DateTimeOffset End { get; }

        public string Status => Start > DateTimeOffset.Now ? "comming" : End > DateTimeOffset.Now ? "on-going" : "finish";

        public string Message { get; }
    }
}
