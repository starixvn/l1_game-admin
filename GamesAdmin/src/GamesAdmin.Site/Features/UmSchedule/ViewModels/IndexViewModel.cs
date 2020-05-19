using System.Collections.Generic;

namespace GamesAdmin.Site.Features.UmSchedule.ViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel(IList<Core.Models.Um.UmSchedule> schedules)
        {
            Schedules = schedules;
        }

        public IList<Core.Models.Um.UmSchedule> Schedules { get; }
    }
}
