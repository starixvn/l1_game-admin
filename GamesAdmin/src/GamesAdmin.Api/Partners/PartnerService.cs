using AutoMapper;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Partners
{
    public interface IPartnerService
    {
        Task<bool> Create(Partner partner);

        Task<IEnumerable<Partner>> GetAll();

        Task<bool> Update(Partner partner);

        Task<bool> Delete(string partnerId);

        Task<Partner> GetById(string partnerId);
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

        public async Task<bool> Create(Partner partner)
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

        public Task<IEnumerable<Partner>> GetAll()
        => Task.Run(() =>
        {
            try
            {
                var users = partnerRepository.AsQueryable().ToList();

                return users.Select(entity => mapper.Map<Partner>(entity));
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Enumerable.Empty<Partner>();
            }
        });

        public async Task<bool> Update(Partner partner)
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

        public async Task<Partner> GetById(string partnerId)
        {
            try
            {
                var partnerEntity = await partnerRepository.FindOneAsync(entity => entity.PartnerId == partnerId);
                return mapper.Map<Partner>(partnerEntity) ;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return null;
            }
        }

    }
}
