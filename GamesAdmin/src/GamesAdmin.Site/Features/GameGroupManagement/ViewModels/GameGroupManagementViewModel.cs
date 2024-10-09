using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.GameGroupManagement.ViewModels
{
    public class GameGroupManagementViewModel
    {
        public IEnumerable<GameGroupViewModel> GameGroups { get; set; } = Enumerable.Empty<GameGroupViewModel>();
    }
}
