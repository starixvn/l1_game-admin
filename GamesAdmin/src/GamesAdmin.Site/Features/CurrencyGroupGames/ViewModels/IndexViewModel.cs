using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.Chips;
using GamesAdmin.Site.Features._Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels
{
    public class IndexViewModel : ViewModelBase
    {
        public string PartnerName { get; set; }

        public string PartnerId { get; set; }

        public string GroupId { get; set; }

        public string GameName { get; set; }

        public GameType GameType { get; set; }

        public List<SelectListItem> GroupSelectList { get; set; }

        public string GroupIdCopy { get; set; }

        public List<SelectListItem> AllCurrencyGroupList { get; set; }

        public bool IsCopySetting { get; set; } = false;

        public IEnumerable<int> OtherGameIdsInGroup { get; set; } = Enumerable.Empty<int>();

        public IEnumerable<SelectListItem> OtherGamesInGroup { get; set; }
    }
}