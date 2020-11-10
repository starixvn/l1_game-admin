namespace GamesAdmin.Core.Models.GameConfigs
{
    public class StakeLevel
    {
        public StakeLevel(double stake, double jackpot) 
        {
            Stake = stake;
            Jackpot = jackpot;
        }

        public double Stake { get; }

        public double Jackpot { get; }
    }
}
