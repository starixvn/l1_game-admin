using AutoMapper;
using GamesAdmin.Core.Models.Partner;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Partner
{
    public interface IPartnerService
    {
        Task<bool> Create(PartnerInfo partner);

        Task<IEnumerable<PartnerInfo>> GetAll();

        Task<bool> Update(PartnerInfo partner);

        Task<bool> Delete(string partnerId);
    }

    public class PartnerService : IPartnerService
    {
        private readonly IGenericRepository<PartnerEntity> partnerRepository;
        private readonly IMapper mapper;
        private readonly ISentryClient sentryClient;

        public PartnerService(
            ISentryClient sentryClient, 
            IMapper mapper,
            IGenericRepository<PartnerEntity> partnerRepository)
        {
            this.sentryClient = sentryClient;
            this.partnerRepository = partnerRepository;
            this.mapper = mapper;
        }

        public async Task<bool> Create(PartnerInfo partner)
        {
            try
            {
                var existPartner = await partnerRepository.FindOneAsync(entity => entity.PartnerId == partner.PartnerId);

                if(existPartner != null)
                {
                    return false;
                }

                var entity = mapper.Map<PartnerEntity>(partner);

                await partnerRepository.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }

            return true;
        }

        public async Task<bool> Delete(string partnerId)
        {
            try
            {
                await partnerRepository.DeleteOneAsync(entity => entity.PartnerId == partnerId);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public Task<IEnumerable<PartnerInfo>> GetAll()
        => Task.Run(() =>
        {
            try
            {
                var users = partnerRepository.AsQueryable().ToList();

                return users.Select(entity => mapper.Map<PartnerInfo>(entity));
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Enumerable.Empty<PartnerInfo>();
            }
        });

        public async Task<bool> Update(PartnerInfo partner)
        {
            var userEntity = await partnerRepository.FindOneAsync(entity => entity.PartnerId.Equals(partner.PartnerId));

            if (userEntity != null)
            {
                var update = Builders<PartnerEntity>.Update.Set(field => field.Name, partner.Name)
                    .Set(field => field.SecretKey, partner.SecretKey)
                    .Set(field => field.MobiVersionUrl, partner.MobiVersionUrl)
                    .Set(field => field.CasinoVersionUrl, partner.CasinoVersionUrl)
                    .Set(field => field.MiniVersionUrl, partner.MiniVersionUrl)
                    .Set(field => field.ApiUrl, partner.ApiUrl);

                await partnerRepository.UpdateAsync(user => user.PartnerId == partner.PartnerId, update);

                return true;
            }

            return false;

        }
    }
}
