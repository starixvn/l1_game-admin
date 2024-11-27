using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Api.Promotions.Requests;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Promotion;
using MediatR;

namespace GamesAdmin.Api.Promotions
{
    public class PromotionHandler
        : IRequestHandler<GetPromotionByStatusRequest, IEnumerable<PromotionModel>>
        , IRequestHandler<GetPromotionByGameIdRequest, IEnumerable<PromotionModel>>
        , IRequestHandler<CreatePromotionRequest, bool>
        , IRequestHandler<GetPromotionByIdRequest, PromotionModel>
        , IRequestHandler<UpdatePromotionRequest, bool>
        , IRequestHandler<DeletePromotionRequest, bool>
        , IRequestHandler<GetAnnouncementByStatusRequest, IEnumerable<Notification>>
        , IRequestHandler<CreateAnnouncementsRequest, bool>
        , IRequestHandler<GetAnnouncementByIdRequest, Notification>
        , IRequestHandler<UpdateAnnouncementRequest, bool>
        , IRequestHandler<DeleteAnnouncementRequest, bool>
        , IRequestHandler<GetPromotionCampaignByStatusRequest, IEnumerable<PromotionConfigModel>>
        , IRequestHandler<GetPromotionCampaignByIdRequest, PromotionConfigModel>
        , IRequestHandler<CreateTournamentRequest, bool>
        , IRequestHandler<GetLeaderboardRequest, IEnumerable<AwardWinningPlayer>>
        , IRequestHandler<GetTournamentWinningPlayersRequest, IEnumerable<AwardWinningPlayer>>
        , IRequestHandler<UpdateTournamentRequest, bool>
        , IRequestHandler<AwardTournamentRequest, bool>
        , IRequestHandler<DeleteTournamentRequest, bool>
        , IRequestHandler<AwardTestCurrencyRequest, bool>
        , IRequestHandler<GetPromotionCampaignByIdsRequest, IEnumerable<PromotionConfigModel>>
        , IRequestHandler<GetPromotionByIdsRequest, IEnumerable<PromotionModel>>
    {
        private readonly IPromotionService promotionService;
        private readonly INotificationService notificationService;
        private readonly IPromotionCampaignService promotionCampaignService;
        private readonly IAwardService awardService;

        public PromotionHandler(
            IPromotionService promotionService,
            INotificationService notificationService,
            IPromotionCampaignService promotionCampaignService,
            IAwardService awardService)
        {
            this.promotionService = promotionService;
            this.notificationService = notificationService;
            this.promotionCampaignService = promotionCampaignService;
            this.awardService = awardService;
        }

        public Task<IEnumerable<PromotionModel>> Handle(GetPromotionByStatusRequest request, CancellationToken cancellationToken)
        => promotionService.GetByStatus(request.Status);

        public Task<IEnumerable<PromotionModel>> Handle(GetPromotionByGameIdRequest request, CancellationToken cancellationToken)
        => promotionService.GetByGameId(request.GameId);

        public Task<bool> Handle(CreatePromotionRequest request, CancellationToken cancellationToken)
        => promotionService.Create(request.Model);

        public Task<PromotionModel> Handle(GetPromotionByIdRequest request, CancellationToken cancellationToken)
        => promotionService.GetById(request.Id);

        public Task<bool> Handle(UpdatePromotionRequest request, CancellationToken cancellationToken)
        => promotionService.Update(request.Model);

        public Task<bool> Handle(DeletePromotionRequest request, CancellationToken cancellationToken)
        => promotionService.Delete(request.Id);

        public Task<IEnumerable<Notification>> Handle(GetAnnouncementByStatusRequest request, CancellationToken cancellationToken)
        => notificationService.GetPromotionAnnouncementByStatus(request.Status, request.Type);

        public Task<bool> Handle(CreateAnnouncementsRequest request, CancellationToken cancellationToken)
        => notificationService.CreatePromotionAnnouncements(request.Models);

        public Task<Notification> Handle(GetAnnouncementByIdRequest request, CancellationToken cancellationToken)
        => notificationService.GetPromotionAnnouncementById(request.Id);

        public Task<bool> Handle(UpdateAnnouncementRequest request, CancellationToken cancellationToken)
        => notificationService.UpdatePromotionAnnouncement(request.Model);

        public Task<bool> Handle(DeleteAnnouncementRequest request, CancellationToken cancellationToken)
        => notificationService.Delete(request.Id);

        public Task<IEnumerable<PromotionConfigModel>> Handle(GetPromotionCampaignByStatusRequest request, CancellationToken cancellationToken)
        => promotionCampaignService.GetByStatus(request.Status);

        public Task<PromotionConfigModel> Handle(GetPromotionCampaignByIdRequest request, CancellationToken cancellationToken)
        => promotionCampaignService.GetById(request.Id);

        public Task<bool> Handle(CreateTournamentRequest request, CancellationToken cancellationToken)
            => promotionCampaignService.Create(request.Model);

        public Task<IEnumerable<AwardWinningPlayer>> Handle(GetLeaderboardRequest request, CancellationToken cancellationToken)
            => promotionCampaignService.GetLeaderboard(request);

        public Task<IEnumerable<AwardWinningPlayer>> Handle(GetTournamentWinningPlayersRequest request, CancellationToken cancellationToken)
           => promotionCampaignService.GetTournamentWinningPlayers(request.PromotionId);

        public Task<bool> Handle(UpdateTournamentRequest request, CancellationToken cancellationToken)
            => promotionCampaignService.Update(request.Model, request.Type);

        public Task<bool> Handle(AwardTournamentRequest request, CancellationToken cancellationToken)
           => awardService.AwardTournament(request);

        public Task<bool> Handle(DeleteTournamentRequest request, CancellationToken cancellationToken)
           => promotionCampaignService.Delete(request.Id);

        public Task<bool> Handle(AwardTestCurrencyRequest request, CancellationToken cancellationToken)
        => awardService.AwardTestCurrency(request);

        public Task<IEnumerable<PromotionConfigModel>> Handle(GetPromotionCampaignByIdsRequest request, CancellationToken cancellationToken)
        => promotionCampaignService.GetByIds(request.Ids);

        public Task<IEnumerable<PromotionModel>> Handle(GetPromotionByIdsRequest request, CancellationToken cancellationToken)
        => promotionService.GetByIds(request.Ids);
    }
}