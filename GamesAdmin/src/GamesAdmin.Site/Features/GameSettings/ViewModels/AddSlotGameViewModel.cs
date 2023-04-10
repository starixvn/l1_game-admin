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

        public AddSlotGameViewModel(IEnumerable<GeneralGameSetting> existingGames, GameType type)
        {
            Type = type;
            ExistingGames = existingGames.Select(game => new SelectListItem($"{game.DisplayName}{GetJackpotText(game.IsEnabledJackpot)}", game.GameId.ToString()));
        }

        public string GetJackpotText(bool isEnableJackpot)
           => isEnableJackpot ? " (Jackpot)" : "";

        public IEnumerable<SelectListItem> ExistingGames { get; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Game Id")]
        public int GameId { get; set; }

        public string Name { get; set; }

        [DisplayName("Clone From")]
        public byte SelectedGame { get; set; }

        public GameType Type { get; set; }
    }
}
