namespace GamesAdmin.Database.Report.Entities
{
    public class BitcoinMasterBetWithUnwindAccountsEntity : StakeBalancedBetWithUnwindAccountsEntity
    {
        public decimal BaseAmount { get; set; }

        public decimal BaseRefund { get; set; }

        public decimal BaseBetWin { get; set; }
    }
}
