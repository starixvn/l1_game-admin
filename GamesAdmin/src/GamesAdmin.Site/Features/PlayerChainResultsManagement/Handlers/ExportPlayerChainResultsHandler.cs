using System;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.PlayerChainResultsManagement.Requests;
using GamesAdmin.Site.Features.PlayerChainResultsManagement.Services;
using GamesAdmin.Site.Features.PlayerChainResultsManagement.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GamesAdmin.Site.Features.PlayerChainResultsManagement.Handlers
{
    public class ExportPlayerChainResultsHandler : IRequestHandler<ExportPlayerChainResultsRequest, byte[]>
    {
        private readonly IPlayerChainResultService playerChainResultService;
        private readonly ILogger<ExportPlayerChainResultsHandler> logger;

        public ExportPlayerChainResultsHandler(IPlayerChainResultService playerChainResultService, ILogger<ExportPlayerChainResultsHandler> logger)
        {
            this.playerChainResultService = playerChainResultService;
            this.logger = logger;
        }

        public async Task<byte[]> Handle(ExportPlayerChainResultsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var query = new PlayerChainResultsQuery
                {
                    GameId = request.GameId,
                    PlayerId = request.PlayerId,
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    ExcludeUUS = request.ExcludeUUS
                };

                var excelData = await playerChainResultService.ExportPlayerChainResults(query);

                if (excelData == null || excelData.Length == 0)
                {
                    return new byte[0];
                }

                return excelData;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred in export handler for GameId: {GameId}, PlayerId: {PlayerId}", request.GameId, request.PlayerId);
                throw new InvalidOperationException($"Failed to export player chain results: {ex.Message}", ex);
            }
        }
    }
}
