using AutoMapper;
using GamesAdmin.Api.ChatBot;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
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
using System.Threading.Tasks;

namespace GamesAdmin.Api.Promotions
{

    public interface IPromotionCampaignService
    {
        Task<IEnumerable<PromotionConfigModel>> GetByStatus(string status);

        //Task<bool> Create(PromotionConfigModel model);

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
        private readonly IChatBotService chatBotService;
        private readonly INotificationService notificationService;

        public PromotionCampaignService(IGenericRepository<PromotionConfigEntity> promotionCampaignRepository,
            ISentryClient sentryClient,
            IMapper mapper,
            IGenericRepository<CurrencyGroupEntity> currencyGroupRepository,
            INotificationService notificationService)
        {
            this.promotionCampaignRepository = promotionCampaignRepository;
            this.sentryClient = sentryClient;
            this.mapper = mapper;
            this.currencyGroupRepository = currencyGroupRepository;
            this.notificationService = notificationService;
        }

        public async Task<IEnumerable<PromotionConfigModel>> GetByStatus(string status)
        {
            try
            {
                List<PromotionConfigEntity> entities;
                if (status.Equals(PromotionCampaignStatus.AllValue))
                {
                    entities = promotionCampaignRepository.AsQueryable()
                        .OrderByDescending(promotion => promotion.StartDateTime)
                        .ToList();
                }
                else
                {
                    entities = (await promotionCampaignRepository.FilterByAsync(entity => entity.Status.Equals(status)))
                        .OrderByDescending(promotion => promotion.StartDateTime)
                        .ToList();
                }

                // Flatten the list of currency group IDs
                var currencyGroupIds = entities
                    .SelectMany(e => e.Operators)
                    .SelectMany(o => o.CurrencyGroups)
                    .Select(cg => cg.CurrencyGroupId)
                    .Distinct()
                    .ToList();

                var objectIdCurrencyGroupIds = currencyGroupIds.Select(id => new ObjectId(id)).ToList();
                // Fetch all necessary currencies in a single query
                var currencies = await currencyGroupRepository.FilterByAsync(currency => objectIdCurrencyGroupIds.Contains(currency.Id));
                var currencyDict = currencies.ToDictionary(c => c.Id.ToString(), c => c.Name);

                // Map entities to models and set currency names
                var promotions = entities.Select(entity =>
                {
                    var promotionModel = mapper.Map<PromotionConfigModel>(entity);

                    foreach (var operatorModel in promotionModel.Operators)
                    {
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

                for (var index = 0; index < promotions.Count; index++)
                {
                    var announcement = await notificationService.GetPromotionAnnouncementByPromotionId(promotions[index].Id);
                    promotions[index].HasAnnouncement = announcement != null;
                }
                return promotions;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }

        public async Task<PromotionConfigModel> GetById(string id)
        {
            var promotion = await promotionCampaignRepository.FindByIdAsync(id);
            return mapper.Map<PromotionConfigModel>(promotion);
        }

    }
}
