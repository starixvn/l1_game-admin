using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Core.Models.Um;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Partners.Requests;
using Microsoft.AspNetCore.Mvc.Rendering;
using Refit;

namespace GamesAdmin.Site.Features.Partners
{
    public interface IPartnerApi : IBaseAuthorizationApi
    {
        [Get("/partner")]
        Task<IEnumerable<Partner>> GetAll();

        [Post("/partner/delete")]
        Task<bool> Delete(string partnerId);

        [Post("/partner/add")]
        Task<bool> Add(Partner partner);

        [Post("/partner/update")]
        Task<bool> Update(Partner partner);

        [Get("/partner/{partnerId}")]
        Task<Partner> GetById(string partnerId);

        [Get("/partner/config")]
        Task<PartnerConfig> GetPartnerConfig(string partnerId, string type);

        [Post("/partner/config")]
        Task<bool> UpdatePartnerConfig(PartnerConfig partnerConfig);

        [Post("/um/partners")]
        Task<bool> UpdatePartnerUM([Body] UpdatePartnerUMRequest request);

        [Get("/um/allUMInfo")]
        Task<IEnumerable<UmInfo>> GetAllUMInfo();
    }

    public interface IPartnerService
    {
        Task<ResultBase> Add(Partner partner);

        Task<IEnumerable<Partner>> GetAll();

        Task<ResultBase> Delete(string partnerId);

        Task<ResultBase> Update(Partner partner);

        Task<Partner> GetById(string partnerId);

        Task<IEnumerable<SelectListItem>> GetPartnersForSelection(Func<Partner, bool> criteria);

        Task<string[]> GetPartnerSiteExclusion(string partnerId, int gameId);

        Task<bool> UpdatePartnerSiteExclusion(string partnerId, int gameId, string[] exclusionSites);

        Task<bool> UpdatePartnerUM(UpdatePartnerUMRequest request);

        Task<IEnumerable<UmInfo>> GetAllUMInfo();
    }

    public class PartnerService : IPartnerService
    {
        private readonly IPartnerApi partnerApi;
        private readonly IMapper mapper;

        public PartnerService(IPartnerApi partnerApi, IMapper mapper)
        {
            this.partnerApi = partnerApi;
            this.mapper = mapper;
        }

        public async Task<ResultBase> Add(Partner partner)
        {
            var error = string.Empty;
            try
            {
                var existPartner = await partnerApi.GetById(partner.PartnerId);
                if (existPartner != null)
                {
                    error = "Partner Id is existed";
                }
                else
                {
                    var addResult = await partnerApi.Add(partner);

                    if (!addResult)
                    {
                        error = "Cannot add new partner";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return new ResultBase(error);
        }

        public async Task<ResultBase> Delete(string partnerId)
        {
            var inserted = await partnerApi.Delete(partnerId);

            return new ResultBase(!inserted ? "Cannot add new partner" : string.Empty);
        }

        public Task<IEnumerable<Partner>> GetAll()
        => partnerApi.GetAll();

        public Task<Partner> GetById(string partnerId)
        => partnerApi.GetById(partnerId);

        public async Task<IEnumerable<SelectListItem>> GetPartnersForSelection(Func<Partner, bool> criteria)
        {
            var partners = await GetAll();
            var filteredPartners = partners.Where(criteria);

            return mapper.Map<IEnumerable<SelectListItem>>(filteredPartners);
        }

        public async Task<ResultBase> Update(Partner partner)
        {
            var updated = await partnerApi.Update(partner);

            return new ResultBase(!updated ? "Cannot update partner" : string.Empty);
        }

        public async Task<string[]> GetPartnerSiteExclusion(string partnerId, int gameId)
        {
            var partnerSiteExclusion = await partnerApi.GetPartnerConfig(partnerId, PartnerConfigType.SiteExclusion.Value);
            var gameSiteExclusion = partnerSiteExclusion?.Value != null && partnerSiteExclusion.Value.ContainsKey(gameId.ToString()) 
                ? partnerSiteExclusion.Value[gameId.ToString()]
                : new List<string>();

            return gameSiteExclusion?.ToArray() ?? new string[] { };
        }

        public async Task<bool> UpdatePartnerSiteExclusion(string partnerId, int gameId, string[] exclusionSites)
        {
            var config = await partnerApi.GetPartnerConfig(partnerId, PartnerConfigType.SiteExclusion.Value);

            if (config != null)
            {
                config.Value[gameId.ToString()] = exclusionSites.ToList();
            }
            else
            {
                config = new PartnerConfig(partnerId, PartnerConfigType.SiteExclusion.Value,
                    new Dictionary<string, IList<string>>()
                    { { gameId.ToString(), exclusionSites.ToList() }});
            }

            return await partnerApi.UpdatePartnerConfig(config);
        }

        public Task<bool> UpdatePartnerUM(UpdatePartnerUMRequest request)
            => this.partnerApi.UpdatePartnerUM(request);

        public Task<IEnumerable<UmInfo>> GetAllUMInfo()
            => this.partnerApi.GetAllUMInfo();
    }
}