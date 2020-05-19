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
            DateTime start, 
            DateTime end, 
            string message = "")
        {
            Start = start;
            End = end;
            Message = message;
        }

        public DateTime Start { get; }

        public DateTime End { get; }

        public string Status => Start > DateTime.Now ? "comming" : End > DateTime.Now ? "on-going" : "finish";

        public string Message { get; }
    }
}
