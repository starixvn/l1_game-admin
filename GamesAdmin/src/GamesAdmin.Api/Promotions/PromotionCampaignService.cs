using AutoMapper;
using GamesAdmin.Api._Shared.Helpers;
using GamesAdmin.Api.ChatBot;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Api.Partners;
using GamesAdmin.Api.Promotions.Requests;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Currencies;
using GamesAdmin.Database.Entities.Promotion;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Promotions
{
    public interface IPromotionCampaignService
    {
        Task<IEnumerable<PromotionConfigModel>> GetByStatus(string status);

        Task<bool> Create(PromotionConfigModel model);

        Task<PromotionConfigModel> GetById(string id);

        Task<bool> Update(PromotionConfigModel model, string type);

        Task<bool> Delete(string id);

        Task<IEnumerable<AwardWinningPlayer>> GetLeaderboard(GetLeaderboardRequest request);
    }

    public class PromotionCampaignService : IPromotionCampaignService
    {
        private readonly IGenericRepository<PromotionConfigEntity> promotionCampaignRepository;
        private readonly IGenericRepository<PromotionLeaderboardEntity> promotionLeaderboardRepository;
        private readonly ISentryClient sentryClient;
        private readonly IMapper mapper;
        private readonly IGenericRepository<CurrencyGroupEntity> currencyGroupRepository;
        private readonly IPartnerService partnerService;
        private readonly INotificationService notificationService;
        private readonly IChatBotService chatBotService;

        public PromotionCampaignService(IGenericRepository<PromotionConfigEntity> promotionCampaignRepository,
            IGenericRepository<PromotionLeaderboardEntity> promotionLeaderboardRepository,
            ISentryClient sentryClient,
            IMapper mapper,
            IGenericRepository<CurrencyGroupEntity> currencyGroupRepository,
            INotificationService notificationService,
            IPartnerService partnerService,
            IChatBotService chatBotService)
        {
            this.promotionCampaignRepository = promotionCampaignRepository;
            this.promotionLeaderboardRepository = promotionLeaderboardRepository;
            this.sentryClient = sentryClient;
            this.mapper = mapper;
            this.currencyGroupRepository = currencyGroupRepository;
            this.notificationService = notificationService;
            this.partnerService = partnerService;
            this.chatBotService = chatBotService;
        }

        public async Task<IEnumerable<PromotionConfigModel>> GetByStatus(string status)
        {
            try
            {
                var entities = await GetPromotionEntitiesByStatus(status);
                var currencyDict = await GetCurrencyDictionary(entities);
                var partnerDict = await GetPartnersDictionary(entities);

                var promotions = MapEntitiesToModels(entities, currencyDict, partnerDict);
                await SetPromotionAnnouncements(promotions);

                return promotions;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }

        private async Task<List<PromotionConfigEntity>> GetPromotionEntitiesByStatus(string status)
        {
            Func<IQueryable<PromotionConfigEntity>, IQueryable<PromotionConfigEntity>> orderBy = query => query.Where(q=> q.Status != "deleted").OrderByDescending(promotion => promotion.StartDateTime);

            // Define the filters for each status
            var filters = new Dictionary<string, Expression<Func<PromotionConfigEntity, bool>>>
            {
                { PromotionCampaignStatus.RunningValue, entity => entity.StartDateTime <= DateTime.UtcNow && DateTime.UtcNow <= entity.EndDateTime },
                { PromotionCampaignStatus.NotStartedValue, entity => DateTime.UtcNow <= entity.StartDateTime },
                { PromotionCampaignStatus.CompletedValue, entity => DateTime.UtcNow > entity.EndDateTime }
            };

            if (status.Equals(PromotionCampaignStatus.AllValue))
            {
                return orderBy(promotionCampaignRepository.AsQueryable()).ToList();
            }

            if (filters.TryGetValue(status, out var filter))
            {
                var filteredEntities = await promotionCampaignRepository.FilterByAsync(filter);
                return orderBy(filteredEntities.AsQueryable()).ToList();
            }

            throw new ArgumentException("Invalid status value", nameof(status));
        }

        private async Task<Dictionary<string, string>> GetCurrencyDictionary(List<PromotionConfigEntity> entities)
        {
            var currencyGroupIds = entities
                .SelectMany(e => e.Operators)
                .SelectMany(o => o.CurrencyGroups)
                .Select(cg => cg.CurrencyGroupId)
                .Distinct()
                .ToList();

            var objectIdCurrencyGroupIds = currencyGroupIds.Select(id => new ObjectId(id)).ToList();
            var currencies = await currencyGroupRepository.FilterByAsync(currency => objectIdCurrencyGroupIds.Contains(currency.Id));
            return currencies.ToDictionary(c => c.Id.ToString(), c => c.Name);
        }

        private async Task<Dictionary<string, string>> GetPartnersDictionary(List<PromotionConfigEntity> entities)
        {
            var operatorIds = entities
               .SelectMany(e => e.Operators)
               .Select(cg => cg.OperatorId)
               .Distinct()
               .ToList();

            var partnerIds = operatorIds.Select(id => id).ToList();
            var partners = await partnerService.GetByIdsAsync(partnerIds);
            return partners.ToDictionary(c => c.PartnerId, c => c.Name);
        }

        private List<PromotionConfigModel> MapEntitiesToModels(List<PromotionConfigEntity> entities, Dictionary<string, string> currencyDict, Dictionary<string, string> partnerDict)
        {
            return entities.Select(entity =>
            {
                var promotionModel = mapper.Map<PromotionConfigModel>(entity);

                foreach (var operatorModel in promotionModel.Operators)
                {
                    if (partnerDict.TryGetValue(operatorModel.OperatorId, out var partnerName))
                    {
                        operatorModel.OperatorName = partnerName;
                    }
                    foreach (var currencyGroup in operatorModel.CurrencyGroups)
                    {
                        if (currencyDict.TryGetValue(currencyGroup.CurrencyGroupId, out var currencyName))
                        {
                            currencyGroup.CurrencyName = currencyName;
                        }
                    }
                }

                return promotionModel;
            }).ToList();
        }

        private async Task SetPromotionAnnouncements(List<PromotionConfigModel> promotions)
        {
            foreach (var promotion in promotions)
            {
                var announcement = await notificationService.GetPromotionAnnouncementByPromotionId(promotion.Id);
                promotion.HasAnnouncement = announcement != null;
            }
        }

        public async Task<PromotionConfigModel> GetById(string id)
        {
            var promotion = await promotionCampaignRepository.FindByIdAsync(id);
            return mapper.Map<PromotionConfigModel>(promotion);
        }

        public async Task<bool> Create(PromotionConfigModel model)
        {
            var entity = mapper.Map<PromotionConfigEntity>(model);
            var partners = await partnerService.GetAll();
            var partnerIds = partners
                .Select(e => e.PartnerId)
                .Distinct()
                .ToList();

            var currencyGroups = await currencyGroupRepository.FilterByAsync(currency => partnerIds.Contains(currency.PartnerId));
            var currencyList = currencyGroups.ToList();

            entity.Operators = partners.Select(partner => new Operator
            {
                OperatorId = partner.PartnerId,
                IncludedSiteNames = "",
                CurrencyGroups = new List<CurrencyGroup>()
            }).ToList();
            var promotion = this.GetPromotionWithMaxIdAsync();
            entity.Status = "new";
            entity.PromotionId = promotion != null ? promotion.PromotionId + 1 : 1;
            entity.UpdatedTime = DateTime.UtcNow;
            await promotionCampaignRepository.InsertOneAsync(entity);
            await chatBotService.NotifyCreatePromotionTournament(model, PromotionTournamentTypeUpdate.CreateTournament);
            return true;
        }

        public PromotionConfigEntity GetPromotionWithMaxIdAsync()
        {
            return promotionCampaignRepository.AsQueryable().OrderByDescending(p => p.PromotionId).First();
        }

        public async Task<bool> Update(PromotionConfigModel model, string type)
        {
            var entity = await promotionCampaignRepository.FindByIdAsync(model.Id);

            if (entity == null)
            {
                return false;
            }

            var oldConfig = mapper.Map<PromotionConfigModel>(entity);
            var modelMapper = mapper.Map<PromotionConfigEntity>(model);
            var changedSettings = SettingHelper.GetPromotionTournamentChangeModel(oldConfig, model);
            sentryClient.CaptureChangedSettings("Promotion Tournament", "Configs", string.Empty, changedSettings);

            UpdateDefinition<PromotionConfigEntity> update;

            if (type.Equals(PromotionTournamentTypeUpdate.OperatorSetting))
            {
                update = Builders<PromotionConfigEntity>.Update
                    .Set(field => field.Operators, modelMapper.Operators);
            }
            else
            {
                update = Builders<PromotionConfigEntity>.Update
                    .Set(field => field.Name, modelMapper.Name)
                    .Set(field => field.StartDateTime, modelMapper.StartDateTime)
                    .Set(field => field.EndDateTime, modelMapper.EndDateTime)
                    .Set(field => field.WinningPrizes, modelMapper.WinningPrizes);
            }

            await promotionCampaignRepository.UpdateAsync(promotion => promotion.Id == entity.Id, update);
            await chatBotService.NotifyUpdatePromotionTournament(model, type);

            return true;
        }

        public async Task<IEnumerable<AwardWinningPlayer>> GetLeaderboard(GetLeaderboardRequest request)
        {
            var promotion = await promotionCampaignRepository.FindOneAsync(e => e.PromotionId == request.PromotionId);
            var awardWinningPlayers = await promotionLeaderboardRepository
                .Collection()
                .Find(x => x.PromotionId == request.PromotionId && x.IsTestCurrency == request.IsTestCurrency)
                .SortByDescending(p => p.PointInBaseCurrency)
                .ThenBy(p => p.DataSourceTimeStamp)
                .ToListAsync();

            return awardWinningPlayers.Select((entity, index) =>
            {
                var data = mapper.Map<AwardWinningPlayer>(entity);
                data.Ranking = index + 1;
                data.CompletedPromotion = promotion.Status == PromotionCampaignStatus.CompletedValue;
                return data;
            }).Where(e => string.IsNullOrEmpty(request.PlayerName) || e.PlayerName == request.PlayerName || e.PlayerId == request.PlayerName);
        }

        public async Task<bool> Delete(string id)
        {
            var entity = await promotionCampaignRepository.FindByIdAsync(id);
            if (entity != null)
            {
                var update = Builders<PromotionConfigEntity>.Update.Set(field => field.Status, PromotionStatus.Deleted.Value);
                await promotionCampaignRepository.UpdateAsync(promotion => promotion.Id.Equals(ObjectId.Parse(id)), update);
                return true;
            }
            return false;
        }
    }
}