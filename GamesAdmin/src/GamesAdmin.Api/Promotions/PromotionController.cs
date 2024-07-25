using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Promotions.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Core.OWResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Promotions
{
    [Route("api/promotion")]
    public class PromotionController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public PromotionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("getByStatus")]
        public Task<IEnumerable<PromotionModel>> GetByStatus(string status)
           => mediator.Send(new GetPromotionByStatusRequest(status));

        [HttpGet("getByGameId")]
        public Task<IEnumerable<PromotionModel>> GetByGameId(GameId gameId)
           => mediator.Send(new GetPromotionByGameIdRequest(gameId));

        [HttpPost("create")]
        public Task<bool> Create(PromotionModel model)
           => mediator.Send(new CreatePromotionRequest(model));

        [HttpPost("createTournament")]
        public Task<bool> CreateTournament(PromotionConfigModel model)
        => mediator.Send(new CreateTournamentRequest(model));

        [HttpGet("getById")]
        public Task<PromotionModel> GetById(string id)
           => mediator.Send(new GetPromotionByIdRequest(id));

        [HttpPost("update")]
        public Task<bool> Update(PromotionModel model)
           => mediator.Send(new UpdatePromotionRequest(model));

        [HttpPost("delete")]
        public Task<bool> Delete(string id)
           => mediator.Send(new DeletePromotionRequest(id));

        [HttpGet("getAnnouncementByStatus")]
        public Task<IEnumerable<Notification>> GetAnnouncementByStatus(string status)
           => mediator.Send(new GetAnnouncementByStatusRequest(status));

        [HttpPost("createAnnouncement")]
        public Task<bool> CreateAnnouncement(IEnumerable<Notification> models)
           => mediator.Send(new CreateAnnouncementsRequest(models));

        [HttpGet("getAnnouncementById")]
        public Task<Notification> GetAnnouncementById(string id)
           => mediator.Send(new GetAnnouncementByIdRequest(id));

        [HttpPost("updateAnnouncement")]
        public Task<bool> UpdateAnnouncement(Notification model)
           => mediator.Send(new UpdateAnnouncementRequest(model));

        [HttpPost("deleteAnnouncement")]
        public Task<bool> DeleteAnnouncement(string id)
           => mediator.Send(new DeleteAnnouncementRequest(id));

        [HttpGet("getCampaignByStatus")]
        public Task<IEnumerable<PromotionConfigModel>> GetCampaignByStatus(string status)
           => mediator.Send(new GetPromotionCampaignByStatusRequest(status));

        [HttpGet("getCampaignById")]
        public Task<PromotionConfigModel> GetCampaignById(string id)
           => mediator.Send(new GetPromotionCampaignByIdRequest(id));

        [HttpPost("award")]
        public async Task<OWResult> Award(AwardWinningPlayerRequest request)
        {
            var awardPlayer = await mediator.Send(request);
            // TODO: correct return type
            return new OWResult { ErrorCode = awardPlayer.IsAwarded ? 0 : 1 };
        }

        [HttpGet("leaderboard")]
        public Task<IEnumerable<AwardWinningPlayer>> GetLeaderboard(int promotionId, string playerName, bool isTestCurrency)
           => mediator.Send(new GetLeaderboardRequest(promotionId, playerName, isTestCurrency));

        [HttpGet("tournamentWinningPlayers")]
        public Task<IEnumerable<AwardWinningPlayer>> GetTournamentWinningPlayers(int promotionId)
          => mediator.Send(new GetTournamentWinningPlayersRequest(promotionId));

        [HttpPost("awardTournament")]
        public Task<bool> AwardTournament(AwardTournamentRequest request)
            => mediator.Send(request);

        [HttpPost("awardTestCurrency")]
        public Task<bool> AwardTestCurrency(AwardTestCurrencyRequest request)
            => mediator.Send(request);

        [HttpPost("updateTournament")]
        public Task<bool> UpdateTournament(PromotionConfigModel model, string type)
           => mediator.Send(new UpdateTournamentRequest(model, type));

        [HttpPost("deleteTournament")]
        public Task<bool> DeleteTournament(string id)
           => mediator.Send(new DeleteTournamentRequest(id));
    }
}