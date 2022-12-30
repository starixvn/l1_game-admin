using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features._Shared.Components.SelectGame
{
    public class SelectGameModel
    {
        public SelectGameModel(string name, IEnumerable<SelectListItem> items, bool selectAll)
        {
            Name = name;
            Items = items;
            SelectAll = selectAll;
        }

        public string Name { get; }

        public IEnumerable<SelectListItem> Items { get; }
    
        public bool SelectAll { get; }
    }
}