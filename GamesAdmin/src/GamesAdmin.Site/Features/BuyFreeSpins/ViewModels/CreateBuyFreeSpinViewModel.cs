using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GamesAdmin.Site.Features.BuyFreeSpins.ViewModels
{
    public class CreateBuyFreeSpinViewModel
    {
        public CreateBuyFreeSpinViewModel()
        {
        }

        public CreateBuyFreeSpinViewModel(IEnumerable<GeneralGameSetting> allSlotGames, IEnumerable<string> operatorIds)
        {
            GameNames = allSlotGames.Select(game => new SelectListItem(game.DisplayName, game.GameId.ToString())).ToList();
            OperatorItems = operatorIds.Select(operatorId => new SelectListItem(operatorId, operatorId)).ToList();
        }

        public bool Enabled { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Number range [1..1000]")]
        public int Multiplier { get; set; }

        public BuyFreeSpinSiteExclusion[] SiteExclusions { get; set; }

        public int GameId { get; set; }

        public List<SelectListItem> OperatorItems { get; set; }
        public List<SelectListItem> GameNames { get; set; }

    }
}