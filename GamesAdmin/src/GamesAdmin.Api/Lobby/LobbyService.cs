using AutoMapper;
using GamesAdmin.Api.GeneralGameSettings;
using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using GamesAdmin.Database.Entities.Lobby;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentry;
using Sentry.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Lobby
{
    public interface ILobbyService
    {

        Task<string> CreateOrUpdate(LobbyModel lobbyModel);

        Task<IEnumerable<LobbyModel>> GetAll();

        Task<LobbyModel> Get(string id);

        Task<bool> Delete(string id);
    }

    public class LobbyService : ILobbyService
    {
        private readonly IGenericRepository<LobbyEntity> lobbyRepository;
        private readonly ISentryClient sentryClient;
        private readonly IMapper mapper;
        private readonly IGenericRepository<CurrencyGroupEntity> currencyGroupRepository;
        private readonly IGeneralGameSettingService generalGameSettingService;

        public LobbyService(IGenericRepository<LobbyEntity> lobbyRepository,
            ISentryClient sentryClient,
            IMapper mapper,
            IGenericRepository<CurrencyGroupEntity> currencyGroupRepository,
            IGeneralGameSettingService generalGameSettingService)
        {
            this.lobbyRepository = lobbyRepository;
            this.sentryClient = sentryClient;
            this.mapper = mapper;
            this.currencyGroupRepository = currencyGroupRepository;
            this.generalGameSettingService = generalGameSettingService;
        }

        public async Task<string> CreateOrUpdate(LobbyModel lobbyModel)
        {
            try
            {
                var lobbyEntity = mapper.Map<LobbyEntity>(lobbyModel);

                var existingLobbyEntity = await lobbyRepository.FindOneAsync(entity => entity.Id == lobbyEntity.Id);

                if (existingLobbyEntity == null)
                {
                    sentryClient.CaptureMessage(string.Format("Added Lobby \"{0}\"", lobbyModel.Name), SentryLevel.Error);
                    await lobbyRepository.InsertOneAsync(lobbyEntity);
                }
                else
                {
                    var currencyGroups = (await currencyGroupRepository.FilterByAsync(entity => true)).ToList();
                    var gameSettings = await generalGameSettingService.GetAll();
                    var log = LobbyLog.BuildChangedLog(existingLobbyEntity, lobbyEntity, currencyGroups, gameSettings.ToList());

                    sentryClient.CaptureMessage(string.Format("Updated Lobby \"{0}\": \n\n{1}", lobbyModel.Name, log), SentryLevel.Error);
                    var update = Builders<LobbyEntity>.Update
                        .Set(field => field.Name, lobbyEntity.Name)
                        .Set(field => field.CurrencyGroupIds, lobbyEntity.CurrencyGroupIds)
                        .Set(field => field.Categories, lobbyEntity.Categories)
                        .Set(field => field.MiniPanelGameIds, lobbyEntity.MiniPanelGameIds)
                        .Set(field => field.Banners, lobbyEntity.Banners);

                    await lobbyRepository.UpdateAsync(entity => entity.Id == lobbyEntity.Id, update);
                }

                return lobbyEntity.Id.ToString();
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return null;
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                var lobbyConfig = await lobbyRepository.FindOneAsync(entity => entity.Id == ObjectId.Parse(id));
                sentryClient.CaptureMessage(string.Format("Deleted Lobby \"{0}\"", lobbyConfig.Name), SentryLevel.Error);
                await lobbyRepository.DeleteOneAsync(entity => entity.Id == ObjectId.Parse(id));

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<LobbyModel> Get(string id)
        {

            try
            {
                var lobbyEntity = await lobbyRepository.FindOneAsync(entity => entity.Id == ObjectId.Parse(id));
                var lobbyModel = mapper.Map<LobbyModel>(lobbyEntity);
                return lobbyModel;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }

        public async Task<IEnumerable<LobbyModel>> GetAll()
        {
            try
            {
                var lobbyEntities = await Task.Run(() => lobbyRepository.AsQueryable().ToList());

                var lobbyModels = lobbyEntities.Select(lobbyEntity => mapper.Map<LobbyModel>(lobbyEntity));
                return lobbyModels;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Enumerable.Empty<LobbyModel>();
            }
        }
    }
}
