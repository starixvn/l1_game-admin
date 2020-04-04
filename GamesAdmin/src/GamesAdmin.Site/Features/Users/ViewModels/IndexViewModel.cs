using GamesAdmin.Core.Models;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Users.ViewModels
{
    public class IndexViewModel
    {
        public IList<User> Users { get; set; }
    }
}
