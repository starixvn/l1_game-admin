namespace GamesAdmin.Core.Models.GameConfigs
{
    public class StakeLevel
    {
        public StakeLevel() { }

        public StakeLevel(double stake, double jackpot) 
        {
            Stake = stake;
            Jackpot = jackpot;
        }

        public double Stake { get; set; }

        public double Jackpot { get; set; }
    }
}
