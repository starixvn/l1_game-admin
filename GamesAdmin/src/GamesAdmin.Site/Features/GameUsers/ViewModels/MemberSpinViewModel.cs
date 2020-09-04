using System.Collections.Generic;
using GamesAdmin.Core.Models.Spins;

namespace GamesAdmin.Site.Features.GameUsers.ViewModels
{
    public class MemberSpinViewModel
    {
        public MemberSpinViewModel(Spin spin, List<SpinLog> logs) 
        {
            Spin = spin;
            Logs = logs;
        }

        public Spin Spin { get; }

        public List<SpinLog> Logs { get; }
    }
}
