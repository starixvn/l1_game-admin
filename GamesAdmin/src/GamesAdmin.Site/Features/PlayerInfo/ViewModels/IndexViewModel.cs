namespace GamesAdmin.Site.Features.PlayerInfo.ViewModels
{
    using GamesAdmin.Core.Models;
    using System.Collections.Generic;
    public class IndexViewModel
    {
        public string PlayerId { get; set; }

        public string Name { get; set; }

        public IEnumerable<PlayerInfoModel> Players { get; set; } = new List<PlayerInfoModel>();
    }
}
