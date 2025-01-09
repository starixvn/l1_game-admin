using System.Collections.Generic;
using System.ComponentModel;

namespace GamesAdmin.Core.Models.GameConfigs
{
	public class TournamentConfig
	{
        [DisplayName("Tournament Code")]
        public string Code { get; set; }

        [DisplayName("Tournament Teams")]
        public List<string> Teams { get; set; }
    }
}
