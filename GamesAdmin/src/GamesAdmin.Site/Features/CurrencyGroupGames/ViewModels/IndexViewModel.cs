using System.Collections.Generic;
using GamesAdmin.Core.Models.Chips;
using GamesAdmin.Site.Features._Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels
{
    public class IndexViewModel : ViewModelBase
    {
        public string PartnerName { get; set; }

        public string PartnerId { get; set; }

        public string CurrencyGroupId { get; set; }

        public int GameId { get; set; }

        public List<SelectListItem> GroupSelectList { get; set; }

        public IEnumerable<ChipModel> ChipConfigs { get; set; }
    }
}