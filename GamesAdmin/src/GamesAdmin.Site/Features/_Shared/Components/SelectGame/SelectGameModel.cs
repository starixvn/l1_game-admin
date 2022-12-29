using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features._Shared.Components.SelectGame
{
    public class SelectGameModel
    {
        public SelectGameModel(string name, IEnumerable<SelectListItem> items)
        {
            Name = name;
            Items = items;
        }

        public string Name { get; }

        public IEnumerable<SelectListItem> Items { get; }
    }
}