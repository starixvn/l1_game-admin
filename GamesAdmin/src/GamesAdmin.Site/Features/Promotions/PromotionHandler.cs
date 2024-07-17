using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.GameSettings;
using GamesAdmin.Site.Features.Promotions.Requests;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions
{
    public class PromotionHandler : IRequestHandler<GetPromotionByStatusRequest, IEnumerable<PromotionModel>>
        , IRequestHandler<CreatePromotionRequest, MessageResult>
        , IRequestHandler<GetPromotionByIdRequest, UpdatePromotionViewModel>
        , IRequestHandler<UpdatePromotionRequest, MessageResult>
        , IRequestHandler<DeletePromotionRequest, bool>
        , IRequestHandler<GetAnnouncementByStatusRequest, IEnumerable<AnnouncementViewModel>>
        , IRequestHandler<CreateAnnouncementsRequest, bool>
        , IRequestHandler<GetAnnouncementByIdRequest, CreateAnnouncementItemViewModel>
        , IRequestHandler<UpdateAnnouncementRequest, bool>
        , IRequestHandler<DeleteAnnouncementRequest, bool>
        , IRequestHandler<GetTournamentPromotionsByStatusRequest, IEnumerable<PromotionConfigViewModel>>
    {
        private readonly IPromotionService promotionService;
        private readonly IPromotionCampaignService promotionCampaignService;
        private readonly IMapper mapper;
        private readonly IAppSettings appSettings;

        public PromotionHandler(IPromotionService promotionService, IGameSettingService gameService, IMapper mapper, IAppSettings appSettings,
            IPromotionCampaignService promotionCampaignService)
        {
            this.promotionService = promotionService;
            this.mapper = mapper;
            this.appSettings = appSettings;
            this.promotionCampaignService = promotionCampaignService;
        }

        public async Task<IEnumerable<PromotionModel>> Handle(GetPromotionByStatusRequest request, CancellationToken cancellationToken)
        {
            var promotions = (await promotionService.GetByStatus(request.Status)).ToList();
            foreach (var promotion in promotions)
            {
                promotion.StartDateTime = TimeZoneInfo.ConvertTimeFromUtc(promotion.StartDateTime, appSettings.DefaultTimeZone);
                promotion.EndDateTime = TimeZoneInfo.ConvertTimeFromUtc(promotion.EndDateTime, appSettings.DefaultTimeZone);
            }
            return promotions;
        }

        public async Task<MessageResult> Handle(CreatePromotionRequest request, CancellationToken cancellationToken)
        {
            var messageResult = await Validate(request.Model);
            if (messageResult.Success)
            {
                var success = await this.promotionService.Create(mapper.Map<PromotionModel>(request.Model));
                return new MessageResult(success, string.Empty);
            }
            return messageResult;
        }

        public async Task<UpdatePromotionViewModel> Handle(GetPromotionByIdRequest request, CancellationToken cancellationToken)
        {
            var promotion = await promotionService.GetById(request.Id);
            return mapper.Map<UpdatePromotionViewModel>(promotion);
        }

        public async Task<MessageResult> Handle(UpdatePromotionRequest request, CancellationToken cancellationToken)
        {
            var messageResult = await Validate(request.Model);
            if (messageResult.Success)
            {
                var success = await this.promotionService.Update(mapper.Map<PromotionModel>(request.Model));
                return new MessageResult(success, string.Empty);
            }
            return messageResult;
        }

        public Task<bool> Handle(DeletePromotionRequest request, CancellationToken cancellationToken)
        => promotionService.Delete(request.Id);

        private async Task<MessageResult> Validate(PromotionViewModel viewModel)
        {
            var model = mapper.Map<PromotionModel>(viewModel);
            bool invalidDailyTime = viewModel.IsDaily && viewModel.StartTime >= viewModel.EndTime;
            if (model.StartDateTime >= model.EndDateTime || invalidDailyTime)
            {
                return new MessageResult(false, "Invalid date or time");
            }
            else
            {
                var existingPromotions = await this.promotionService.GetByGameId(viewModel.GameId);
                if (existingPromotions.Any(promotion => !ValidateDateTime(model, promotion)))
                {
                    return new MessageResult(false, "Overlap date or time with another promotion");
                }
            }
            return new MessageResult(true, string.Empty);
        }

        private bool ValidateDateTime(PromotionModel model, PromotionModel existingPromotion)
        {
            if (!string.IsNullOrEmpty(model.PromotionId) && model.PromotionId.Equals(existingPromotion.PromotionId))
            {
                return true;
            }
            if (model.StartDateTime >= existingPromotion.EndDateTime || model.EndDateTime <= existingPromotion.StartDateTime)
            {
                return true;
            }
            else
            {
                return ValidateTime(model, existingPromotion);
            }
        }

        private bool ValidateTime(PromotionModel model, PromotionModel existingPromotion)
        {
            if (model.IsDaily)
            {
                if (existingPromotion.IsDaily)
                {
                    return ValidateTimeDaily(model, existingPromotion);
                }
                else
                {
                    return ValidateTimeConsecutiveAndDaily(existingPromotion, model);
                }
            }
            else
            {
                if (existingPromotion.IsDaily)
                {
                    return ValidateTimeConsecutiveAndDaily(model, existingPromotion);
                }
                return false;
            }
        }

        private bool ValidateTimeDaily(PromotionModel model, PromotionModel existingPromotion)
        {
            var startTime = model.StartDateTime.TimeOfDay;
            var endTime = model.EndDateTime.TimeOfDay;
            var existingStartTime = existingPromotion.StartDateTime.TimeOfDay;
            var existingEndTime = existingPromotion.EndDateTime.TimeOfDay;
            return startTime > existingEndTime || endTime < existingStartTime;
        }

        private bool ValidateTimeConsecutiveAndDaily(PromotionModel consecutive, PromotionModel daily)
        {
            var consecutiveStartTime = consecutive.StartDateTime.TimeOfDay;
            var consecutiveEndTime = consecutive.EndDateTime.TimeOfDay;
            var dailyStartTime = daily.StartDateTime.TimeOfDay;
            var dailyEndTime = daily.EndDateTime.TimeOfDay;
            if ((consecutive.StartDateTime - consecutive.EndDateTime).Days == 1)
            {
                if (consecutiveEndTime > consecutiveStartTime)
                {
                    return consecutiveEndTime < dailyStartTime || consecutiveStartTime > dailyEndTime;
                }
                else
                {
                    return consecutiveEndTime < dailyStartTime && consecutiveStartTime > dailyEndTime;
                }
            }
            return false;
        }

        public async Task<IEnumerable<AnnouncementViewModel>> Handle(GetAnnouncementByStatusRequest request, CancellationToken cancellationToken)
        {
            var announcements = (await promotionService.GetAnnouncementByStatus(request.Status))
                .Select(announcement => mapper.Map<AnnouncementViewModel>(announcement))
                .ToList();
            foreach (var a in announcements)
            {
                if (!string.IsNullOrEmpty(a.PromotionId))
                {
                    if (a.DisplayKind.Value == NotificationDisplayKind.Promotion.Value)
                    {
                        var promotion = await promotionService.GetById(a.PromotionId);
                        a.Promotion = mapper.Map<PromotionViewModel>(promotion);
                        a.IsHappyHours = true;
                    }
                    else if (a.DisplayKind.Value == NotificationDisplayKind.PromotionCampaign.Value)
                    {
                        var campaign = await promotionCampaignService.GetCampaignById(a.PromotionId);
                        a.PromotionCampaign = mapper.Map<PromotionCampaignViewModel>(campaign);
                    }
                }
            }
            return announcements;
        }

        public async Task<bool> Handle(CreateAnnouncementsRequest request, CancellationToken cancellationToken)
        {
            var models = request.Models.Select(model => mapper.Map<Notification>(model)).ToList();
            for (var index = 0; index < models.Count; index++)
            {
                if (!string.IsNullOrEmpty(models[index].PromotionId))
                {
                    var promotion = await promotionService.GetById(models[index].PromotionId);
                    models[index].GameName = Game.FromGameId(promotion.GameId).Value;
                    models[index].GameId = (int)promotion.GameId;
                }
            }
            return await this.promotionService.CreateAnnouncements(models);
        }

        public async Task<CreateAnnouncementItemViewModel> Handle(GetAnnouncementByIdRequest request, CancellationToken cancellationToken)
        {
            var announcement = await promotionService.GetAnnouncementById(request.Id);
            return mapper.Map<CreateAnnouncementItemViewModel>(mapper.Map<AnnouncementViewModel>(announcement));
        }

        public async Task<bool> Handle(UpdateAnnouncementRequest request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<Notification>(request.Model);
            if (!string.IsNullOrEmpty(model.PromotionId))
            {
                var promotion = await promotionService.GetById(model.PromotionId);
                if(promotion != null)
                {
                    model.GameName = Game.FromGameId(promotion.GameId).Value;
                    model.GameId = (int)promotion.GameId;
                }
            }
            return await promotionService.UpdateAnnouncement(model);
        }

        public Task<bool> Handle(DeleteAnnouncementRequest request, CancellationToken cancellationToken)
        => promotionService.DeleteAnnouncement(request.Id);

        public async Task<IEnumerable<PromotionConfigViewModel>> Handle(GetTournamentPromotionsByStatusRequest request, CancellationToken cancellationToken)
        {
            var promotions = (await promotionCampaignService.GetCampaignByStatus(request.Status)).ToList();
            ConvertPromotionTimesToDefaultTimeZone(promotions);
            var promotionViewModels = MapPromotionsToViewModels(promotions);
            return promotionViewModels;
        }

        private void ConvertPromotionTimesToDefaultTimeZone(List<PromotionConfigModel> promotions)
        {
            foreach (var promotion in promotions)
            {
                promotion.StartDateTime = TimeZoneInfo.ConvertTimeFromUtc(promotion.StartDateTime, appSettings.DefaultTimeZone);
                promotion.EndDateTime = TimeZoneInfo.ConvertTimeFromUtc(promotion.EndDateTime, appSettings.DefaultTimeZone);
            }
        }

        private List<PromotionConfigViewModel> MapPromotionsToViewModels(List<PromotionConfigModel> promotions)
        {
            var promotionViewModels = promotions.Select(promotion =>
            {
                var promotionViewModel = mapper.Map<PromotionConfigViewModel>(promotion);
                return promotionViewModel;
            }).ToList();

            return promotionViewModels;
        }
    }
}