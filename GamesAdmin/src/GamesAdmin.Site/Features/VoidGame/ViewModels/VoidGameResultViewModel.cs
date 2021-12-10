using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.OWResults;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.VoidGame.ViewModels
{
    public class VoidGameResultViewModel
    {
        public VoidGameResultViewModel(VoidGameResult result, List<SelectListItem> partners)
        {
            Result = result;
            Partners = partners;
        }

        public List<SelectListItem> Partners { get; set; }


        public VoidGameResult Result { get; set; }
    }
}