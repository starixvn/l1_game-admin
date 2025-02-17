using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.GameSettings.ViewModels;

namespace GamesAdmin.Site.Features.OperateSettings.ViewModels
{
    public class CurrencyGroupGameSettingViewModel
    {
        public IEnumerable<CurrencyGroupGame> CurrencyGroupGames { get; set; }  

        public StatusViewModel AllGameStatusViewModel { get; set; }


        public IEnumerable<ExportCurrencyGroupViewModel> BuildExportModel()
        {
            var exportModel = AllGameStatusViewModel.CurrencyGroups.Select(currencyGroup => new ExportCurrencyGroupViewModel
            {
                OperatorId = currencyGroup.PartnerId,
                CurrencyGroupName = currencyGroup.Name,
                CurrencyGroupGames = AllGameStatusViewModel.CurrencyGroupGames.Where(groupGame => groupGame.GroupId == currencyGroup.Id).ToList(),
            });

            return exportModel;
        }
    }


    public class ExportCurrencyGroupViewModel
    {
        public string OperatorId { get; set; }

        public string CurrencyGroupName { get; set; }

        public IEnumerable<CurrencyGroupGame> CurrencyGroupGames { get; set; }

    }
}
