using System;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.PlayerChainResultsManagement.Requests;
using GamesAdmin.Site.Features.PlayerChainResultsManagement.Services;
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
                var excelData = await playerChainResultService.ExportPlayerChainResults(request.GameId, request.PlayerId, request.FromDate, request.ToDate, request.ExcludeUUS);

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
