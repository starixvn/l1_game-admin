using AutoMapper;
using GamesAdmin.Api.GeneralGameSettings;
using GamesAdmin.Api.RoundWatcher;
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

        Task<Game[]> UniqueRoundTimerGameAllMarket();

        Task<string> CreateOrUpdateDirectLobby(DirectLobbyModel model);

        Task<IEnumerable<DirectLobbyModel>> GetAllDirectLobby();

        Task<DirectLobbyModel> GetDirectLobby(string id);

        Task<bool> DeleteDirectLobby(string id);
    }

    public class LobbyService : ILobbyService
    {
        private readonly IGenericRepository<LobbyEntity> lobbyRepository;
        private readonly IGenericRepository<DirectLobbyEntity> directLobbyRepository;
        private readonly ISentryClient sentryClient;
        private readonly IMapper mapper;
        private readonly IGenericRepository<CurrencyGroupEntity> currencyGroupRepository;
        private readonly IGeneralGameSettingService generalGameSettingService;
        private readonly IRoundWatcherService roundWatcherService;

        public LobbyService(IGenericRepository<LobbyEntity> lobbyRepository,
            IGenericRepository<DirectLobbyEntity> directLobbyRepository,
            ISentryClient sentryClient,
            IMapper mapper,
            IGenericRepository<CurrencyGroupEntity> currencyGroupRepository,
            IGeneralGameSettingService generalGameSettingService,
            IRoundWatcherService roundWatcherService)
        {
            this.lobbyRepository = lobbyRepository;
            this.directLobbyRepository = directLobbyRepository;
            this.sentryClient = sentryClient;
            this.mapper = mapper;
            this.currencyGroupRepository = currencyGroupRepository;
            this.generalGameSettingService = generalGameSettingService;
            this.roundWatcherService = roundWatcherService;
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
                        .Set(field => field.Banners, lobbyEntity.Banners)
                        .Set(field => field.RoundTimerGame, lobbyEntity.RoundTimerGame);

                    await lobbyRepository.UpdateAsync(entity => entity.Id == lobbyEntity.Id, update);
                }
              
                if (HasRoundTimerChanged(existingLobbyEntity?.RoundTimerGame, lobbyEntity.RoundTimerGame))
                {
                    await NotifyRoundTimerWatcher();
                }
                return lobbyEntity.Id.ToString();
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return null;
        }

        private static bool HasRoundTimerChanged(RoundTimerGameEntity oldRoundTimer, RoundTimerGameEntity newRoundTimer)
        {
            return !EqualityComparer<RoundTimerGameEntity>.Default.Equals(oldRoundTimer, newRoundTimer);
        }

        private async Task<RoundWatcherResult> NotifyRoundTimerWatcher()
        {
            var games = await UniqueRoundTimerGameAllMarket();
            var roundWatcherRequest = new RoundWatcherRequest(games);
            return await roundWatcherService.NotifyToWatcher(roundWatcherRequest);
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

        public async Task<Game[]> UniqueRoundTimerGameAllMarket()
        {
            try
            {
                var lobbies = await GetAll();
                var uniqueGames = lobbies
                                  .SelectMany(lobby => lobby.RoundTimerGame?.Games ?? Enumerable.Empty<Game>())
                                  .GroupBy(game => game.Id)
                                  .Select(group => group.First())
                                  .ToArray();
                return uniqueGames;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Array.Empty<Game>();
            }
        }

        public async Task<string> CreateOrUpdateDirectLobby(DirectLobbyModel model)
        {
            try
            {
                var directLobbyEntity = mapper.Map<DirectLobbyEntity>(model);

                var existingLobbyEntity = await directLobbyRepository.FindOneAsync(entity => entity.Id == directLobbyEntity.Id);

                if (existingLobbyEntity == null)
                {
                    sentryClient.CaptureMessage(string.Format("Added Direct Lobby \"{0}\"", model.Name), SentryLevel.Error);
                    await directLobbyRepository.InsertOneAsync(directLobbyEntity);
                }
                else
                {
                    var currencyGroups = (await currencyGroupRepository.FilterByAsync(entity => true)).ToList();
                    var gameSettings = await generalGameSettingService.GetAll();
                    var log = LobbyLog.BuildDirectLobbyChangedLog(existingLobbyEntity, directLobbyEntity, currencyGroups, gameSettings.ToList());
                    sentryClient.CaptureMessage(string.Format("Updated Direct Lobby \"{0}\": \n\n{1}", model.Name, log), SentryLevel.Error);
                    var update = Builders<DirectLobbyEntity>.Update
                        .Set(field => field.Name, directLobbyEntity.Name)
                        .Set(field => field.CurrencyGroupIds, directLobbyEntity.CurrencyGroupIds)
                        .Set(field => field.GameIds, directLobbyEntity.GameIds)
                        .Set(field => field.NewGames, directLobbyEntity.NewGames);

                    await directLobbyRepository.UpdateAsync(entity => entity.Id == directLobbyEntity.Id, update);
                }
                return directLobbyEntity.Id.ToString();
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return null;
        }

        public async Task<IEnumerable<DirectLobbyModel>> GetAllDirectLobby()
        {
            try
            {
                var lobbyEntities = await Task.Run(() => directLobbyRepository.AsQueryable().ToList());

                var lobbyModels = lobbyEntities.Select(lobbyEntity => mapper.Map<DirectLobbyModel>(lobbyEntity));
                return lobbyModels;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Enumerable.Empty<DirectLobbyModel>();
            }
        }

        public async Task<DirectLobbyModel> GetDirectLobby(string id)
        {
            try
            {
                var lobbyEntity = await directLobbyRepository.FindOneAsync(entity => entity.Id == ObjectId.Parse(id));
                var lobbyModel = mapper.Map<DirectLobbyModel>(lobbyEntity);
                return lobbyModel;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }

        public async Task<bool> DeleteDirectLobby(string id)
        {
            try
            {
                var lobbyConfig = await directLobbyRepository.FindOneAsync(entity => entity.Id == ObjectId.Parse(id));
                sentryClient.CaptureMessage(string.Format("Deleted Lobby \"{0}\"", lobbyConfig.Name), SentryLevel.Error);
                await directLobbyRepository.DeleteOneAsync(entity => entity.Id == ObjectId.Parse(id));

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }
    }
}
