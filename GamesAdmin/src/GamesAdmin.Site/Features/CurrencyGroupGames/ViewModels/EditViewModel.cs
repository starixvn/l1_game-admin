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

        public double[] BetLevels { get; set; }

        public StakeLevel[] StakeLevels { get; set; }


        public bool DisplayMinMaxBet => new HashSet<GameType>
        {
            GameType.AutoRNG,
            GameType.AutoRNGIndex,
            GameType.SlotRNG,
            GameType.Crash,
            GameType.DynamicOdds,
            GameType.Marble,
            GameType.CrashTurnBase,
            GameType.GuaranteedWin,
            GameType.AutoRNGIndex,
            0
        }.Contains(GameType);

        public List<MaxBetChoiceViewModel> MaxBetChoiceSetting { get; set; }

        public bool DisplayMaxBetPerChoice => MaxBetChoiceSetting != null && MaxBetChoiceSetting.Count > 2;

        public double[] CoinValues { get; set; }

        public double? DefaultCoinValue { get; set; }
        public double? DefaultBetLevel { get; set; }

        public double MaxWinPerRound { get; set; }

        public bool CopySettingGame { get; set; }

        public bool DisabledCoinValue(int index)
        {
            return CoinValues[index] != 0 && !CopySettingGame;
        }

        public IEnumerable<int> OtherGameIdsInGroup { get; set; }
    }
}