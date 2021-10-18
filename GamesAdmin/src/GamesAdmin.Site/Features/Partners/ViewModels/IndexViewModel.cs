using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Partners;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Partners.ViewModels
{
    public class IndexViewModel
    {
        public IList<Core.Models.Partners.Partner> Partners { get; set; }
    }
}
