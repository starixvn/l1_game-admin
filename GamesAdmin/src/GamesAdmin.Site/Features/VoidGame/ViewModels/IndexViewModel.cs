using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.OWResults;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.VoidGame.ViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
        }

        [DisplayName("Operator")]
        [Required]
        public string PartnerId { get; set; } = "ONEworks1";

        [DisplayName("Member Id")]
        [Required]
        [Range(1, int.MaxValue)]
        public int MemberId { get; set; } = 1971601;

        [DisplayName("Site Id")]
        [Required]
        public string SiteId { get; set; } = "4100300";

        [DisplayName("Game Round Id")]
        [Required]
        [Range(1, long.MaxValue)]
        public long GameRoundId { get; set; }

        [DisplayName("Game")]
        [Required]
        public byte GameId { get; set; } = 71;

        public List<SelectListItem> GameIds { get; set; }

        public List<SelectListItem> Partners { get; set; }

        [Required]
        public string Reason { get; set; } = "david test";

        public bool FreeBet { get; set; }

        public VoidGameResult Result { get; set; }

        public string MessageSuccessful => Result?.IsRunningTicket == true ? $"Voided successfully the running hands: {Result.HandIds}" : "Voided successfully";
    }
}