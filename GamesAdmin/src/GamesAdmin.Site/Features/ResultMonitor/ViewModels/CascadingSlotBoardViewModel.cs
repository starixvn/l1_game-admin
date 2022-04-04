using GamesAdmin.Core.Models.SlotGame;

namespace GamesAdmin.Site.Features.ResultMonitor.ViewModels
{
    public class CascadingSlotBoardViewModel
    {
        public CascadingSlotBoardViewModel(string name, CascadingSettlementResult settlementResult)
        {
            Name = name;
            SettlementResult = settlementResult;
        }

        public string Name { get; }

        public CascadingSettlementResult SettlementResult { get; }

        public bool IsError => SettlementResult == null;
    }
}