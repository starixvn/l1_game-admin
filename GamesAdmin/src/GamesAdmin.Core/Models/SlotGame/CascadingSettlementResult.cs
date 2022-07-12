using System.Collections.Generic;

namespace GamesAdmin.Core.Models.SlotGame
{
    public class SettlementCascadingSlotResult
    {
        public IEnumerable<CascadingSlotIcon> icons { get; set; }

        public string[] board { get; set; }

        public IEnumerable<int> multipliers { get; set; }

        public int freeSpinCount { get; set; }
    }

    public class CascadingSlotIcon
    {
        public string index { get; set; }

        public int count { get; set; }

        public int odds { get; set; }
    }
}
