using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.BuyFreeSpins.ViewModels
{
    public class UpdateBuyFreeSpinViewModel : CreateBuyFreeSpinViewModel
    {
        public string GameName { get; set; }

        public UpdateBuyFreeSpinViewModel(IEnumerable<string> operatorIds)
        {
            OperatorItems = operatorIds.Select(operatorId => new SelectListItem(operatorId, operatorId)).ToList();
        }
    }
}