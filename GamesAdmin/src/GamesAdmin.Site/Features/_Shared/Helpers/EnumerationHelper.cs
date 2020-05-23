using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Helpers
{
    public static class EnumerationHelper
    {
        public static List<SelectListItem> ToSelectList<T>() where T : Enumeration, new()
        => Enumeration.GetAll<T>().Select(x => new SelectListItem(x.DisplayName, x.Value)).ToList();
    }
}
