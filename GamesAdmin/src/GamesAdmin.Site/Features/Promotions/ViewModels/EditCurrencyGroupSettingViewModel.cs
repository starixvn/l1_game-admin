namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class EditCurrencyGroupSettingViewModel
    {
        public string Id { get; set; }
        public string GroupId { get; set; }

        public string OperatorId { get; set; }

        public bool Enabled { get; set; } = false;

        public double SlotGamesMinBet { get; set; } = 0;

        public double OtherGamesMinBet { get; set; } = 0;

        public string IncludedSiteNames { get; set; }
    }

}