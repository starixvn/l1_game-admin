using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class AddSlotGameViewModel : ViewModelBase
    {
        public AddSlotGameViewModel()
        {
        }

        public AddSlotGameViewModel(IEnumerable<GeneralGameSetting> existingGames, IEnumerable<SelectListItem> gameTypes)
        {
            GameTypes = gameTypes.Where(type => IsAccelerateGameType(type.Value));
            ExistingGames = existingGames.Select(game => new SelectListItem($"{game.DisplayName}{GetJackpotText(game.IsEnabledJackpot)}", game.GameId.ToString()));
        }
        private static bool IsAccelerateGameType(string type)
        {
            var gameType = Enum.Parse<GameType>(type);
            var accelerateTypes = new GameType[] { GameType.Slots, GameType.CascadingSlots, GameType.Crash, GameType.CrashTurnBase };
            return accelerateTypes.Contains(gameType);
        }

        public static string GetJackpotText(bool isEnableJackpot)
           => isEnableJackpot ? " (Jackpot)" : "";

        public IEnumerable<SelectListItem> ExistingGames { get; }


        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Game Id")]
        public int GameId { get; set; }

        public string GameName { get; set; }

        [DisplayName("Clone From")]
        public int SelectedGame { get; set; }

        [DisplayName("Type")]
        public GameType GameType { get; set; }

        public IEnumerable<SelectListItem> GameTypes { get; set; }
    }
}
