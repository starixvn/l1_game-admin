using AutoMapper;
using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Lobby;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Lobby
{
    public interface ILobbyService
    {

        Task<bool> CreateOrUpdate(LobbyModel lobbyModel);

        Task<IEnumerable<LobbyModel>> GetAll();

        Task<LobbyModel> Get(string id);

        Task<bool> Delete(string id);
    }

    public class LobbyService : ILobbyService
    {
        private readonly IGenericRepository<LobbyEntity> lobbyRepository;
        private readonly ISentryClient sentryClient;
        private readonly IMapper mapper;


        public LobbyService(IGenericRepository<LobbyEntity> lobbyRepository, ISentryClient sentryClient, IMapper mapper)
        {
            this.lobbyRepository = lobbyRepository;
            this.sentryClient = sentryClient;
            this.mapper = mapper;
        }

        public async Task<bool> CreateOrUpdate(LobbyModel lobbyModel)
        {
            try
            {
                var lobbyEntity = mapper.Map<LobbyEntity>(lobbyModel);

                var gameConfig = await lobbyRepository.FindOneAsync(entity => entity.Id == lobbyEntity.Id);

                if (gameConfig == null)
                {
                    await lobbyRepository.InsertOneAsync(lobbyEntity);
                }
                else
                {

                    //sentryClient.CaptureChangedSettings(game.Value, "Partner's game setting", $" ({group.PartnerId} - {group.Name})", changedSettings);
                    // TODO log changed settings

                    var update = Builders<LobbyEntity>.Update
                        .Set(field => field.Name, lobbyEntity.Name)
                        .Set(field => field.CurrencyGroupIds, lobbyEntity.CurrencyGroupIds)
                        .Set(field => field.Categories, lobbyEntity.Categories)
                        .Set(field => field.MiniPanelGameIds, lobbyEntity.MiniPanelGameIds);



                    await lobbyRepository.UpdateAsync(entity => entity.Id == lobbyEntity.Id, update);
                }

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
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
