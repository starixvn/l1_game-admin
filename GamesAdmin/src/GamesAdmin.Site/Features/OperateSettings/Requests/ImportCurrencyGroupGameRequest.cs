using System.Collections.Generic;
using GamesAdmin.Core.Models;
namespace GamesAdmin.Site.Features.OperateSettings.Requests
{

    public class ImportCurrencyGroupGameRequest
    {
        public IEnumerable<CurrencyGroupGameImportModel> CurrencyGroupGameImportModels { get; set; }
    }

    public class CurrencyGroupGameImportModel
    {
        public string OperatorId { get; set; }
        public string GroupName { get; set; }
        public List<CurrencyGroupGame> GameSettings { get; set; }
    }
}