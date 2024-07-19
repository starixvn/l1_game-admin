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
        public Task<OWResult> Award(AwardWinningPlayerRequest request)
           => mediator.Send(request);
    }
}