using System.Collections.Generic;
using GamesAdmin.Core.Models.Chips;

namespace GamesAdmin.Site.Features.Chips.ViewModels
{
    public class ChipManagementViewModel
    {
        public IEnumerable<ChipModel> Models { get; set; }
    }
}