using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.GameGroupManagement.ViewModels
{
    public class GameGroupViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<int> GameIds { get; set; }

        public string DisplayGames { get; set; }
    }
}