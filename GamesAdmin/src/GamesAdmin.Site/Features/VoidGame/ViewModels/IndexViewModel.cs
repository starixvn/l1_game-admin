using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.OWResults;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.VoidGame.ViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            GameIds = Enumeration.GetAll<Game>()
                .Where(x => (byte)x.GameId != 0)
                .Select(x => new SelectListItem { Text = x.DisplayName, Value = x.GameId.ToString() })
                .ToList();
        }

        [DisplayName("Operator")]
        [Required]
        public string PartnerId { get; set; }

        [DisplayName("Member Id")]
        [Required]
        [Range(1, int.MaxValue)]
        public int MemberId { get; set; }

        [DisplayName("Site Id")]
        [Required]
        public string SiteId { get; set; }

        [DisplayName("Game Round Id")]
        [Required]
        [Range(1, long.MaxValue)]
        public long GameRoundId { get; set; }

        [DisplayName("Game")]
        [Required]
        public GameId GameId { get; set; }

        public List<SelectListItem> GameIds { get; set; }

        public List<SelectListItem> Partners { get; set; }

        [Required]
        public string Reason { get; set; }

        public bool FreeBet { get; set; }

        public VoidGameResult Result { get; set; }
    }
}