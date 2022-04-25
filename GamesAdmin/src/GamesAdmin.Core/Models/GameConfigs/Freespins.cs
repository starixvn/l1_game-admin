using System.Collections.Generic;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class Freespins
    {
        public Freespins()
        {
        }

        public Freespins(List<FreeSpinsOdds> initialOdds, List<FreeSpinsOdds> additionalOdds)
        {
            InitialOdds = initialOdds;
            AdditionalOdds = additionalOdds;
        }

        public List<FreeSpinsOdds> InitialOdds { get; set; }
        public List<FreeSpinsOdds> AdditionalOdds { get; set; }
    }
}
