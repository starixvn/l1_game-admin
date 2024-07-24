using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class UpdateOperatorSettingViewModel
    {
        public string Id { get; set; }

        public string OperatorName { get; set; }

        public string OperatorId { get; set; }

        public string GroupId { get; set; }

        public List<SelectListItem> GroupSelectList { get; set; }
    }
}