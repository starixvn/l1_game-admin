using System.Collections.Generic;
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
        public List<GameSettingRequest> GameSettings { get; set; }
    }

    public class GameSettingRequest
    {
        public decimal GameId { get; set; }
        public string GameName { get; set; }
        public double MinBet { get; set; }
        public double MaxBet { get; set; }
        public List<string> Chips { get; set; }
        public decimal MaxWinPerRound { get; set; }
        public Dictionary<string, double> MaxBetPerChoices { get; set; }
    }
}