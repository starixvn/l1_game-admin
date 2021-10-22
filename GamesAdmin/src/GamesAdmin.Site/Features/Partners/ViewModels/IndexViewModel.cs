using System.Collections.Generic;
using GamesAdmin.Core.Models.Partners;

namespace GamesAdmin.Site.Features.Partners.ViewModels
{
    public class IndexViewModel
    {
        public IList<Partner> Partners { get; set; }
    }
}