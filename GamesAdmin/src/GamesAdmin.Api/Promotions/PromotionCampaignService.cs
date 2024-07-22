using AutoMapper;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Api.Partners;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Database;
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

        //Task<bool> Update(PromotionConfigModel model);

        //Task<bool> Delete(string id);
    }


    public class PromotionCampaignService : IPromotionCampaignService
    {
        private readonly IGenericRepository<PromotionConfigEntity> promotionCampaignRepository;
        private readonly ISentryClient sentryClient;
        private readonly IMapper mapper;
        private readonly IGenericRepository<CurrencyGroupEntity> currencyGroupRepository;
        private readonly IPartnerService partnerService;
        private readonly INotificationService notificationService;

        public PromotionCampaignService(IGenericRepository<PromotionConfigEntity> promotionCampaignRepository,
            ISentryClient sentryClient,
            IMapper mapper,
            IGenericRepository<CurrencyGroupEntity> currencyGroupRepository,
            INotificationService notificationService,
            IPartnerService partnerService)
        {
            this.promotionCampaignRepository = promotionCampaignRepository;
            this.sentryClient = sentryClient;
            this.mapper = mapper;
            this.currencyGroupRepository = currencyGroupRepository;
            this.notificationService = notificationService;
            this.partnerService = partnerService;
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
            Func<IQueryable<PromotionConfigEntity>, IQueryable<PromotionConfigEntity>> orderBy = query => query.OrderByDescending(promotion => promotion.StartDateTime);

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
                ExcludedSiteNames = "",
                CurrencyGroups = currencyList
                    .Where(c => c.PartnerId == partner.PartnerId)
                    .Select(c => new CurrencyGroup
                    {
                        CurrencyGroupId = c.Id.ToString(),
                        OtherGameMinBet = 2,
                        SlotGameMinBet = 0.2,
                        Enabled = c.Enabled
                    })
                    .ToList()
            }).ToList();
            var promotion = this.GetPromotionWithMaxIdAsync();
            entity.Status = "new";
            entity.PromotionId = promotion != null ? promotion.PromotionId + 1 : 1;
            entity.UpdatedTime = DateTime.UtcNow;
            await promotionCampaignRepository.InsertOneAsync(entity);
            return true;
        }

        public PromotionConfigEntity GetPromotionWithMaxIdAsync()
        {
            return promotionCampaignRepository.AsQueryable().OrderByDescending(p => p.PromotionId).First();
        }
    }
}
