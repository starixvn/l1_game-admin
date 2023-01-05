using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class AddSlotGameViewModel
    {
        public AddSlotGameViewModel(IEnumerable<SlotGame> existingGames)
        {
            Mode = GameMode.Medium;
            BetLevels = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            ExistingGames = existingGames.Select(game => new SelectListItem(game.DisplayName, game.Id.ToString()));
            Stakes = new double[] { 0, 0, 0 };
            CloneSettings = true;
            Type = GameType.Slots;
            ModeItems = Enum.GetValues(typeof(GameMode))
                .Cast<GameMode>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
        }

        public AddSlotGameViewModel()
        {
            Mode = GameMode.Medium;
            BetLevels = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            ExistingGames = new List<SelectListItem>();
            Stakes = new double[] { 0, 0, 0 };
            CloneSettings = true;
            Type = GameType.Slots;
            ModeItems = Enum.GetValues(typeof(GameMode))
                .Cast<GameMode>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
        }

        public IEnumerable<SelectListItem> ExistingGames { get; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Game Id")]
        public int GameId { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Clone Settings")]
        public bool CloneSettings { get; set; }

        [DisplayName("Clone From")]
        public byte SelectedGame { get; set; }

        [DisplayName("Type")]
        public GameType Type { get; set; }

        [DisplayName("Game Enabled")]
        public bool Enabled { get; set; }


        [DisplayName("Bot Enabled")]
        public bool BotEnabled { get; set; }

        [DisplayName("Delay Start Time")]
        public int DelayStartTime { get; set; }

        [DisplayName("Enable Free Bet")]
        public bool EnableFreeBet { get; set; }

        public GameMode Mode { get; set; }

        public List<SelectListItem> ModeItems { get; set; }

        [DisplayName("Number of Slot Icons")]
        public byte NumberOfIcons { get; set; }

        [DisplayName("Max number of odds per Icon")]
        public byte OddsPerIcon { get; set; }

        public double[] Stakes { get; set; }

        public double MinBonus { get; set; }

        public double MaxBonus { get; set; }


        [DisplayName("Big Win Ratio (%)")]
        public double BigWinRatio { get; set; }

        public int[] BetLevels { get; set; }


        public double[] Jackpots => new double[] { 0, 0, 0 };

        public double[] BotRatios => new double[] { 40, 40, 20 };

    }
}