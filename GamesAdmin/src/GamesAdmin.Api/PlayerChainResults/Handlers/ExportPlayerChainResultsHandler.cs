using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.PlayerChainResults.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GamesAdmin.Api.PlayerChainResults.Handlers
{
    public class ExportPlayerChainResultsHandler : IRequestHandler<ExportPlayerChainResultsRequest, byte[]>
    {
        private readonly IMediator mediator;
        private readonly ILogger<ExportPlayerChainResultsHandler> logger;

        public ExportPlayerChainResultsHandler(IMediator mediator, ILogger<ExportPlayerChainResultsHandler> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task<byte[]> Handle(ExportPlayerChainResultsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var getDataRequest = new GetPlayerChainResultsRequest
                {
                    GameId = request.GameId,
                    PlayerId = request.PlayerId,
                    Page = 1,
                    PageSize = int.MaxValue
                };

                var response = await mediator.Send(getDataRequest, cancellationToken);

                if (response?.Results == null || !response.Results.Any())
                {
                    logger.LogWarning("No data found for export with GameId: {GameId}, PlayerId: {PlayerId}", request.GameId, request.PlayerId);
                    return new byte[0];
                }

                var csv = new StringBuilder();
                
                var headers = new[]
                {
                    "Player ID",
                    "Player Name", 
                    "Operator ID",
                    "Game ID",
                    "Chain Name",
                    "Current/Total Board",
                    "Currency",
                    "Assigned At (GMT+7)",
                    "Completed At (GMT+7)",
                    "Status",
                    "Bet Level",
                    "Coin Value",
                    "Total Win Amount",
                    "Total Bet Amount"
                };

                csv.AppendLine(string.Join(",", headers.Select(h => EscapeCsvField(h))));

                foreach (var result in response.Results)
                {
                    var row = new[]
                    {
                        result.PlayerId,
                        result.PlayerName,
                        result.OperatorId,
                        result.GameId,
                        result.ChainName,
                        $"{result.CurrentBoardIndex}/{result.TotalBoards}",
                        result.Currency,
                        result.AssignedAtGMT7,
                        result.CompletedAtGMT7 ?? "-",
                        result.Status,
                        result.BetLevel.ToString(),
                        result.CoinValue.ToString(CultureInfo.InvariantCulture),
                        result.TotalWinAmount.ToString(CultureInfo.InvariantCulture),
                        result.TotalBetAmount.ToString(CultureInfo.InvariantCulture)
                    };

                    csv.AppendLine(string.Join(",", row.Select(field => EscapeCsvField(field))));
                }

                var csvData = Encoding.UTF8.GetBytes(csv.ToString());
                return csvData;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred in export handler for GameId: {GameId}, PlayerId: {PlayerId}", request.GameId, request.PlayerId);
                throw new InvalidOperationException($"Failed to export player chain results: {ex.Message}", ex);
            }
        }

        private static string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
            {
                return "";
            }

            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }

            return field;
        }
    }
}
