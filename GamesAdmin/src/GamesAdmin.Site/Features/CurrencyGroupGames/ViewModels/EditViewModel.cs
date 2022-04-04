using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.Chips;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.GameSettings.ViewModels;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels
{
    public class EditViewModel : ViewModelBase
    {
        public int GameId { get; set; }

        public GameType GameType { get; set; }

        public string GroupId { get; set; }

        public string GameName { get; set; }

        public string PartnerName { get; set; }

        public bool Enabled { get; set; }

        public double MinBet { get; set; }

        public double MaxBet { get; set; }

        public int[] Chips { get; set; }

        public List<ChipModel> AllChips { get; set; }

        public string[] SiteExclusions { get; set; }

        public StakeLevel[] StakeLevels { get; set; }

        public bool DisplayMinMaxBet => GameType == GameType.AutoRNG;

        public List<MaxBetChoiceViewModel> MaxBetChoiceSetting { get; set; }

        public bool DisplayMaxBetPerChoice => MaxBetChoiceSetting != null && MaxBetChoiceSetting.Count > 2;

        public double[] BetLevels { get; set; }
        public double[] CoinValues { get; set; }
        public int Multiplier { get; set; }
        public double MaxWinPerRound { get; set; }
    }
}