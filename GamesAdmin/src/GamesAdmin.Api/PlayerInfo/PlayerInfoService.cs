namespace GamesAdmin.Api.PlayerInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GamesAdmin.Core.Models;
    using GamesAdmin.Database;
    using GamesAdmin.Database.Entities;
    using Sentry;
    public interface IPlayerInfoService
    {
        Task<IEnumerable<PlayerInfoModel>> GetPlayerInfos(string playerId, string name);
    }
    public class PlayerInfoService : IPlayerInfoService
    {
        private readonly IGenericRepository<UserSiloEntity> userSiloRepository;
        private readonly ISentryClient sentryClient;
        public PlayerInfoService(IGenericRepository<UserSiloEntity> userSiloRepository, ISentryClient sentryClient)
        {
            this.userSiloRepository = userSiloRepository;
            this.sentryClient = sentryClient;
        }

        public async Task<IEnumerable<PlayerInfoModel>> GetPlayerInfos(string playerId, string name)
        {
            try
            {
                var playerIdTrimmed = playerId?.Trim();
                var nameTrimmed = name?.Trim()?.ToLower();

                var hasPlayerId = !string.IsNullOrEmpty(playerIdTrimmed);
                var hasName = !string.IsNullOrEmpty(nameTrimmed);

                if (!hasPlayerId && !hasName)
                {
                    return Enumerable.Empty<PlayerInfoModel>();
                }

                var userEntities = await userSiloRepository.FilterByAsync(x =>
                    (!hasPlayerId || (x.PlayerId != null && x.PlayerId.Contains(playerIdTrimmed))) &&
                    (!hasName || (
                        (x.NickName != null && x.NickName.ToLower() == nameTrimmed) ||
                        (x.PlayerName != null && x.PlayerName.ToLower() == nameTrimmed)
                    ))
                );

                return userEntities.Select(entity => new PlayerInfoModel
                {
                    PlayerId = entity.PlayerId ?? "",
                    PlayerName = entity.PlayerName,
                    NickName = entity.NickName,
                    Currency = entity.Currency,
                    OperatorId = entity.OperatorId,
                    SiteName = entity.SiteName
                }).ToList();
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Enumerable.Empty<PlayerInfoModel>();
            }           
        }
    }
}
