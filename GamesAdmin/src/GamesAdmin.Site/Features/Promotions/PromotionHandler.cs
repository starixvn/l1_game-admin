using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.CurrencyGroups;
using GamesAdmin.Site.Features.GameSettings;
using GamesAdmin.Site.Features.Partners;
using GamesAdmin.Site.Features.Promotions.Requests;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        , IRequestHandler<AwardWinningPlayerRequest, OWResult>
        , IRequestHandler<CreateTournamentRequest, MessageResult>
        , IRequestHandler<GetLeaderboardRequest, IEnumerable<AwardWinningPlayer>>
        , IRequestHandler<GetTournamentWinningPlayersRequest, IEnumerable<AwardWinningPlayer>>
        , IRequestHandler<AwardTournamentRequest, bool>
        , IRequestHandler<GetUpdateOperatorSettingRequest, UpdateOperatorSettingViewModel>
        , IRequestHandler<GetCurrencySettingRequest, EditCurrencyGroupSettingViewModel>
        , IRequestHandler<UpdateOperatorSettingRequest, MessageResult>
        , IRequestHandler<UpdateTournamentRequest, MessageResult>
        , IRequestHandler<GetTournamentByIdRequest, PromotionCampaignViewModel>
        , IRequestHandler<DeleteTournamentRequest, bool>
    {
        private readonly IPromotionService promotionService;
        private readonly IPromotionCampaignService promotionCampaignService;
        private readonly IMapper mapper;
        private readonly IAppSettings appSettings;
        private readonly ICurrencyGroupService currencyGroupService;

        public PromotionHandler(IPromotionService promotionService, IGameSettingService gameService, IMapper mapper, IAppSettings appSettings,
            IPromotionCampaignService promotionCampaignService,
            ICurrencyGroupService currencyGroupService)
        {
            this.promotionService = promotionService;
            this.mapper = mapper;
            this.appSettings = appSettings;
            this.promotionCampaignService = promotionCampaignService;
            this.currencyGroupService = currencyGroupService;
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
                    if (promotion != null)
                    {
                        models[index].GameName = Game.FromGameId(promotion.GameId).Value;
                        models[index].GameId = (int)promotion.GameId;
                        models[index].DisplayKind = NotificationDisplayKind.Promotion;
                    }
                    else models[index].DisplayKind = NotificationDisplayKind.PromotionCampaign;
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
                if (promotion != null)
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

        public Task<OWResult> Handle(AwardWinningPlayerRequest request, CancellationToken cancellationToken)
            => promotionCampaignService.Award(request);

        public async Task<MessageResult> Handle(CreateTournamentRequest request, CancellationToken cancellationToken)
        {
            var messageResult = await ValidateTournament(request.Model);
            if (messageResult.Success)
            {
                var success = await this.promotionCampaignService.Create(mapper.Map<PromotionConfigModel>(request.Model));
                return new MessageResult(success, string.Empty);
            }
            return messageResult;
        }

        private async Task<MessageResult> ValidateTournament(PromotionCampaignViewModel viewModel)
        {
            var model = mapper.Map<PromotionConfigModel>(viewModel);
            if (model.StartDateTime >= model.EndDateTime)
            {
                return new MessageResult(false, "Invalid date or time");
            }
            else
            {
                var existingPromotions = await this.promotionCampaignService.GetCampaignByStatus(PromotionCampaignStatus.AllValue);
                if (existingPromotions.Any(promotion => !ValidateTournamentDateTime(model, promotion)))
                {
                    return new MessageResult(false, "Overlap date or time with another promotion");
                }
            }
            return new MessageResult(true, string.Empty);
        }

        private bool ValidateTournamentDateTime(PromotionConfigModel model, PromotionConfigModel existingPromotion)
        {
            if (!string.IsNullOrEmpty(model.Id) && model.Id.Equals(existingPromotion.Id))
            {
                return true;
            }
            if (model.StartDateTime >= existingPromotion.EndDateTime || model.EndDateTime <= existingPromotion.StartDateTime)
            {
                return true;
            }
            return false;
        }

        public Task<IEnumerable<AwardWinningPlayer>> Handle(GetLeaderboardRequest request, CancellationToken cancellationToken)
            => promotionCampaignService.GetLeaderboard(request);

        public Task<IEnumerable<AwardWinningPlayer>> Handle(GetTournamentWinningPlayersRequest request, CancellationToken cancellationToken)
            => promotionCampaignService.GetTournamentWinningPlayers(request);

        public Task<bool> Handle(AwardTournamentRequest request, CancellationToken cancellationToken)
            => promotionCampaignService.AwardTournament(request);

        public async Task<UpdateOperatorSettingViewModel> Handle(GetUpdateOperatorSettingRequest request, CancellationToken cancellationToken)
        {
            var result = new UpdateOperatorSettingViewModel();
            var currencyGroups = (await currencyGroupService.GetByPartner(request.OperatorId, group => group.Enabled)).ToList();
            result.OperatorId = request.OperatorId;
            result.OperatorName = request.OperatorName;
            result.Id = request.Id;
            result.Status = request.Status;
            result.GroupSelectList = currencyGroups.Select(group => new SelectListItem(group.Name, group.Id)).ToList();

            return result;
        }

        public async Task<EditCurrencyGroupSettingViewModel> Handle(GetCurrencySettingRequest request, CancellationToken cancellationToken)
        {
            var result = new EditCurrencyGroupSettingViewModel();
            var promotion = await promotionCampaignService.GetCampaignById(request.Id);
            var currentOperator = promotion.Operators.Where(o => o.OperatorId == request.OperatorId).FirstOrDefault();
            if (currentOperator != null)
            {
                result.IncludedSiteNames = currentOperator.IncludedSiteNames;
                var currencyGroup = currentOperator.CurrencyGroups.FirstOrDefault(cg => cg.CurrencyGroupId == request.GroupId);
                if (currencyGroup != null)
                {
                    result.SlotGamesMinBet = currencyGroup.SlotGameMinBet;
                    result.OtherGamesMinBet = currencyGroup.OtherGameMinBet;
                    result.Enabled = currencyGroup.Enabled;
                }
            }
            result.Status = promotion.Status;
            result.OperatorId = request.OperatorId;
            result.GroupId = request.GroupId;
            result.Id = request.Id;

            return result;
        }

        public async Task<MessageResult> Handle(UpdateOperatorSettingRequest request, CancellationToken cancellationToken)
        {
            var promotion = await promotionCampaignService.GetCampaignById(request.Model.Id);
            MapOperatorSettings(request.Model, promotion);
            var success =  await promotionCampaignService.Update(promotion, PromotionTournamentTypeUpdate.OperatorSetting);
            return new MessageResult(success, string.Empty);
        }

        public void MapOperatorSettings(EditCurrencyGroupSettingViewModel source, PromotionConfigModel destination)
        {
            var operatorModel = destination.Operators.FirstOrDefault(o => o.OperatorId == source.OperatorId);

            if (operatorModel != null)
            {
                operatorModel.IncludedSiteNames = source.IncludedSiteNames;
                var currencyGroup = operatorModel.CurrencyGroups.FirstOrDefault(cg => cg.CurrencyGroupId == source.GroupId);

                if (currencyGroup != null)
                {
                    currencyGroup.SlotGameMinBet = source.SlotGamesMinBet;
                    currencyGroup.OtherGameMinBet = source.OtherGamesMinBet;
                    currencyGroup.Enabled = source.Enabled;
                }
                else
                {
                    var newCurrencyGroup = new CurrencyGroupsModel
                    {
                        CurrencyGroupId = source.GroupId,
                        SlotGameMinBet = source.SlotGamesMinBet,
                        OtherGameMinBet = source.OtherGamesMinBet,
                        Enabled = source.Enabled,
                    };

                    operatorModel.CurrencyGroups.Add(newCurrencyGroup);
                }
            }
            else
            {
                Console.WriteLine("Operator not found.");
            }
        }

        public async Task<MessageResult> Handle(UpdateTournamentRequest request, CancellationToken cancellationToken)
        {
            if (request.Model.Status.Equals(PromotionCampaignStatus.RunningValue))
            {
                return new MessageResult(false, "This promotion is running, are you sure to make the change");
            }
            var result = await this.promotionCampaignService.Update(mapper.Map<PromotionConfigModel>(request.Model), PromotionTournamentTypeUpdate.EditTournament);
            return new MessageResult(result, string.Empty);
        }

        public async Task<PromotionCampaignViewModel> Handle(GetTournamentByIdRequest request, CancellationToken cancellationToken)
        {
            var promotion = await promotionCampaignService.GetCampaignById(request.Id);
            return mapper.Map<PromotionCampaignViewModel>(promotion);
        }

        public Task<bool> Handle(DeleteTournamentRequest request, CancellationToken cancellationToken)
            => promotionCampaignService.Delete(request.Id);
    }
}