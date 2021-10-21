using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Site.Features._Shared;
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
    }

    public interface IPartnerService
    {
        Task<ResultBase> Add(Partner partner);

        Task<IEnumerable<Partner>> GetAll();

        Task<ResultBase> Delete(string partnerId);

        Task<ResultBase> Update(Partner partner);

        Task<Partner> GetById(string partnerId);

        Task<IEnumerable<SelectListItem>> GetPartnersForSelection(Func<Partner, bool> criteria);
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
    }
}