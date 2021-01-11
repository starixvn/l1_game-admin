using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.VoidGame.ViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            GameIds = Enum.GetValues(typeof(GameId)).Cast<GameId>()
                            .Select(v => new SelectListItem { Text = v.ToString(), Value = ((int)v).ToString() })
                            .ToList();
        }

        [DisplayName("Member Id")]
        [Required]
        [Range(1, int.MaxValue)]
        public int MemberId { get; set; }

        [DisplayName("Site Id")]
        [Required]
        [Range(1, int.MaxValue)]
        public int SiteId { get; set; }

        [DisplayName("Game Round Id")]
        [Required]
        [Range(1, long.MaxValue)]
        public long GameRoundId { get; set; }

        [DisplayName("Game")]
        public GameId GameId { get; set; }

        public List<SelectListItem> GameIds { get; set; }

        [Required]
        public string Reason { get; set; }
    }
}