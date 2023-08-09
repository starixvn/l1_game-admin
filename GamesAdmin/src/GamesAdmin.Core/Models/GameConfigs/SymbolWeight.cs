namespace GamesAdmin.Core.Models.GameConfigs
{
    public class SymbolWeight
    {
        public SymbolWeight(byte mode, int[][] weight)
        {
            Mode = mode;
            Weight = weight;
        }

        public byte Mode { get; set; }

        public int[][] Weight { get; set; }
    }
}
