using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class HoldemPokerReportExtraPropsViewModel
    {
        public HoldemPokerReportExtraPropsViewModel(string title, GameId gameId)
        {
            Title = title;
            GameId = gameId;
        }

        public string Title { get; }

        public GameId GameId { get; set; }
    }
}
