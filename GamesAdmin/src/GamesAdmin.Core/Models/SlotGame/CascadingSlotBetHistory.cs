using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class CascadingSlotBetHistory
    {
        public CascadingSlotBetRecord Round { get; set; }

        public IEnumerable<CascadingSlotRoundMultiplier> Multipliers { get; set; }
    }
}
