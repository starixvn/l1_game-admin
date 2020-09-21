namespace GamesAdmin.Core.Models.Spins
{
    public class Spin
    {
        public Spin(int memberId, int availSpins, int usedSpins, bool enabled)
        {
            MemberId = memberId;
            AvailSpins = availSpins;
            UsedSpins = usedSpins;
            Enabled = enabled;
        }

        public int MemberId { get; }

        public int AvailSpins { get; }

        public int UsedSpins { get; }

        public bool Enabled { get; }
    }
}
