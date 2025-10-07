using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using GamesAdmin.Api._Shared.Helpers;
using GamesAdmin.Api.ChatBot;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Api.Notifications.Requests;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.Games
{
    public interface IGameSettingsService
    {
        Task<IEnumerable<GameConfig>> Get();

        Task<GameCreationResult> Create(GameConfig game);

        Task<bool> ImportGame(GameConfig game);

        Task<GameConfig> GetByName(string name);

        Task<bool> UpdateStatus(string[] names, bool enabled);

        Task<bool> UpdateDisabledMessage(string name, Dictionary<string, string> messages, bool isApplyAll = false);

        Task<bool> UpdateBetLimit(int gameId, BetLimit betLimit, IEnumerable<int> otherGameIdsInGroup);

        Task<bool> UpdateOdds(string name, Dictionary<string, double> odds, IEnumerable<int> otherGameIdsInGroup);

        Task<bool> UpdateMarginSettings(string name, IEnumerable<Dictionary<string, double>> marginSettings);

        Task<bool> UpdateBotRatios(string name, double[] ratios, IEnumerable<int> otherGameIdsInGroup);

        Task<bool> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, double>> iconOdds, IEnumerable<int> otherGameIdsInGroup);

        Task<bool> UpdateCascadingSlotOdds(string name, CascadingSlotsConfig cascadingSlotsConfig, IEnumerable<int> otherGameIdsInGroup);

        Task<bool> UpdateJackpot(string name, Jackpot jackpot, List<StakeLevel> stakeLevels, IEnumerable<int> otherGameIdsInGroup);

        Task<bool> UpdateSlotBotSettings(string name, Jackpot jackpot, double[] betLevels, IEnumerable<int> otherGameIdsInGroup);

        Task<bool> UpdateSlotConfig(string name, bool botEnabled, bool enablePromotionRTP, SlotsConfig slotsConfig, IEnumerable<int> otherGameIdsInGroup);

        Task<bool> UpdateCascadingSlotsConfig(string name, bool botEnabled, bool enablePromotionRTP, CascadingSlotsConfig cascadingSlotsConfig, IEnumerable<int> otherGameIdsInGroup);

        Task<bool> UpdateGeneralConfig(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, OddsMultipliersConfig oddsMultipliers = null, IEnumerable<int> otherGameIdsInGroup = null, TournamentConfig tournamentConfig = null, VideoStreamConfig videoStreamConfig = null);

        Task<Dictionary<string, string>> GetDisabledMessage(string name);

        Task<bool> UpdateSlotRNGConfig(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, SlotRNGConfig slotRNGConfig);

        Task<bool> UpdateSymbolWeight(string name, SymbolWeight symbolWeight, IEnumerable<int> otherGameIdsInGroup);

        Task<bool> UpdateCrashConfig(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, CrashConfig crashConfig);

        Task<bool> IsGameIdExists(int gameId);

        Task<bool> IsGameNameExists(string gameName);

        Task<bool> UpdatePlinkoConfig(string name, PlinkoConfig plinkoConfig);
    }

    public class GameSettingsService : IGameSettingsService
    {
        private const byte HoursOfDay = 24;

        private readonly IMapper mapper;

        private readonly ISentryClient sentryClient;

        private readonly IChatBotService chatBotService;

        private readonly IGenericRepository<GameConfigEntity> repository;

        private readonly INotificationService notificationService;

        public GameSettingsService(
            IMapper mapper,
            ISentryClient sentryClient,
            IChatBotService chatBotService,
            IGenericRepository<GameConfigEntity> repository,
            INotificationService notificationService
            )
        {
            this.repository = repository;
            this.sentryClient = sentryClient;
            this.chatBotService = chatBotService;
            this.mapper = mapper;
            this.notificationService = notificationService;
        }

        public async Task<IEnumerable<GameConfig>> Get()
        {
            try
            {
                var gameEntities = await Task.Run(() => repository.AsQueryable().ToList());

                return gameEntities.Select(entity => mapper.Map<GameConfig>(entity));
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Enumerable.Empty<GameConfig>();
            }
        }

        public async Task<GameCreationResult> Create(GameConfig game)
        {
            try
            {
                var gameId = GetGameId(game.Info);

                // Validate GameId and GameName for all game types
                var validationResult = await ValidateGameForCreation(game.Info, gameId);
                if (!validationResult.Success)
                {
                    return validationResult;
                }

                var entity = new GameConfigEntity
                {
                    Name = game.Info.Name,
                    Type = (byte)game.Info.Type,
                    EnableFreeBet = game.Info.EnableFreeBet,
                    Enabled = game.EnableSetting.Enabled,
                    DelayStartTime = game.EnableSetting.DelayStartTime,
                    DisabledMessage = game.EnableSetting.DisabledMessage,
                    Maxbet = game.BetLimit.MaxBet,
                    Minbet = game.BetLimit.MinBet,
                    Botenabled = game.BotSetting.BotEnabled,
                    BotMaxBet = game.BotSetting.BotMaxBet,
                    HoursMaxBot = game.BotSetting.HoursMaxBot,
                    Odds = game.Odds,
                    OddsDisplayNames = game.OddsDisplayNames,
                    Choices = game.Choices,
                    SlotsConfig = game.SlotsConfig,
                    CascadingSlotsConfig = game.CascadingSlotsConfig,
                    SlotRNGConfig = game.SlotRNGConfig,
                    SiteExclusions = new string[] { },
                    Time = DateTime.Now,
                    GameId = gameId,
                    DisplayName = game.Info.DisplayName,
                    CrashConfig = game.CrashConfig,
                    ApplyMultiplier = game.Info.ApplyMultiplier,
                    HaveTournament = game.Info.HaveTournament,
                    HaveVideoStream = game.Info.HaveVideoStream,
                    UseMargin = game.Info.UseMargin
                    // Additional properties will be set by SetAdditionalGameProperties method
                };

                if (game.Info.ApplyMultiplier)
                {
                    entity.OddsMultipliers = new OddsMultipliersConfig()
                    {
                        Values = new List<MultiplierDistribution> { new MultiplierDistribution() }
                    };
                }

                if (game.Info.HaveTournament)
                {
                    entity.TournamentConfig = new TournamentConfig()
                    {
                        Code = "",
                        Teams = new List<string> { }
                    };
                }

                if (game.Info.HaveVideoStream)
                {
                    entity.VideoStreamConfig = new VideoStreamConfig()
                    {
                        VideoInfos = new List<VideoInfo> { }
                    };
                }

                SetAdditionalGameProperties(entity, game);

                await repository.InsertOneAsync(entity);

                var messages = new Dictionary<string, string> { [Language.DefaultLanguageValue] = game.EnableSetting.DisabledMessage };
                await notificationService.AddDisableMessage(game.Info.Name, messages);
                //Notify about new game creation
                await NotifyNewGameCreated(game.Info?.Name, entity);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return GameCreationResult.CreateError(
                    GameCreationErrorCodes.DatabaseError,
                    "An error occurred while saving the game to the database. Please try again.");
            }

            return GameCreationResult.CreateSuccess();
        }

        public async Task<bool> ImportGame(GameConfig gameConfig)
        {
            try
            {
                // Validate game configuration before import
                var gameId = GetGameId(gameConfig.Info);
                var validationResult = await ValidateGameForCreation(gameConfig.Info, gameId);
                if (!validationResult.Success)
                {
                    // Log validation failure for import
                    var validationException = new InvalidOperationException($"Game import validation failed: {validationResult.ErrorMessage}");
                    sentryClient.CaptureException(validationException);
                    return false;
                }

                var entity = new GameConfigEntity
                {
                    Name = gameConfig.Info?.Name,
                    Type = (byte)(gameConfig.Info?.Type ?? 0),
                    EnableFreeBet = gameConfig.Info?.EnableFreeBet ?? false,
                    Enabled = gameConfig.EnableSetting?.Enabled ?? false,
                    DelayStartTime = gameConfig.EnableSetting?.DelayStartTime ?? 0,
                    DisabledMessage = gameConfig.EnableSetting?.DisabledMessage,
                    Maxbet = gameConfig.BetLimit?.MaxBet ?? 0,
                    Minbet = gameConfig.BetLimit?.MinBet ?? 0,
                    Botenabled = gameConfig.BotSetting?.BotEnabled ?? false,
                    BotMaxBet = gameConfig.BotSetting?.BotMaxBet ?? 0,
                    HoursMaxBot = gameConfig.BotSetting?.HoursMaxBot,
                    Odds = gameConfig.Odds,
                    OddsDisplayNames = gameConfig.OddsDisplayNames,
                    Choices = gameConfig.Choices,
                    SlotsConfig = gameConfig.SlotsConfig,
                    CascadingSlotsConfig = gameConfig.CascadingSlotsConfig,
                    SlotRNGConfig = gameConfig.SlotRNGConfig,
                    SiteExclusions = gameConfig.Info?.SiteExclusions,
                    Time = DateTime.Now,
                    GameId = gameId,
                    DisplayName = gameConfig.Info?.DisplayName,
                    CrashConfig = gameConfig.CrashConfig,
                    ApplyMultiplier = gameConfig.Info?.ApplyMultiplier ?? false,
                    HaveTournament = gameConfig.Info?.HaveTournament ?? false,
                    HaveVideoStream = gameConfig.Info?.HaveVideoStream ?? false,
                    UseMargin = gameConfig.Info?.UseMargin ?? false,
                    RandomMode = gameConfig.Info?.RandomMode ?? 1,
                    Margin = gameConfig.Margin,
                    MaxWinPerRound = gameConfig.Info?.MaxWinPerRound ?? 0,
                    MaxBetChoices = gameConfig.BetLimit?.MaxBetChoices,
                    OddsMultipliers = gameConfig.OddsMultipliers,
                    TournamentConfig = gameConfig.TournamentConfig,
                    VideoStreamConfig = gameConfig.VideoStreamConfig
                };

                SetAdditionalGameProperties(entity, gameConfig);

                await repository.InsertOneAsync(entity);

                var messages = new Dictionary<string, string> { [Language.DefaultLanguageValue] = gameConfig.EnableSetting?.DisabledMessage };
                await notificationService.AddDisableMessage(gameConfig.Info?.Name, messages);

                // Notify about new game creation
                await NotifyNewGameCreated(gameConfig.Info?.Name, entity);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }

            return true;
        }

        [SuppressMessage("SonarQube", "S1067", Justification = "Expressions should not be too complex")]
        private int GetGameId(GameInfo gameInfo)
        {
            if (gameInfo.Type == GameType.Slots
                || gameInfo.Type == GameType.Crash
                || gameInfo.Type == GameType.CrashTurnBase
                || gameInfo.Type == GameType.AutoRNGIndex
                || gameInfo.Type == GameType.CascadingSlots
                || gameInfo.Type == GameType.Plinko)
            {
                return gameInfo.GameId;
            }

            return (int)Core.Enumeration.Game.FromGameName(gameInfo.Name).GameId;
        }

        private async Task<GameCreationResult> ValidateGameForCreation(GameInfo gameInfo, int gameId)
        {
            // Check for duplicate GameId
            var gameIdExists = await IsGameIdExists(gameId);
            if (gameIdExists)
            {
                // Capture the duplicate GameId attempt as an exception
                var duplicateGameIdException = new InvalidOperationException($"Game with GameId {gameId} already exists. Cannot create duplicate game: {gameInfo.Name}");
                sentryClient.CaptureException(duplicateGameIdException);

                return GameCreationResult.CreateError(
                    GameCreationErrorCodes.DuplicateGameId,
                    $"Game ID {gameId} already exists. Please choose a different Game ID.");
            }

            // Check for duplicate game name
            var gameNameExists = await IsGameNameExists(gameInfo.Name);
            if (gameNameExists)
            {
                // Capture the duplicate game name attempt as an exception
                var duplicateGameNameException = new InvalidOperationException($"Game with name '{gameInfo.Name}' already exists. Cannot create duplicate game.");
                sentryClient.CaptureException(duplicateGameNameException);

                return GameCreationResult.CreateError(
                    GameCreationErrorCodes.DuplicateGameName,
                    $"Game name '{gameInfo.Name}' already exists. Please choose a different game name.");
            }

            return GameCreationResult.CreateSuccess();
        }

        public async Task<GameConfig> GetByName(string name)
        {
            var gameEntity = await repository.FindOneAsync(entity => entity.Name == name);

            if (gameEntity != null && gameEntity.DisplayName == null)
            {
                gameEntity.DisplayName = Core.Enumeration.Game.FromGameName(name).DisplayName;
            }

            return gameEntity != null ? mapper.Map<GameConfig>(gameEntity) : default;
        }

        public async Task<bool> UpdateStatus(string[] names, bool enabled)
        {
            try
            {
                var update = Builders<GameConfigEntity>.Update.Set(field => field.Enabled, enabled);
                await repository.UpdateManyAsync(config => names.Contains(config.Name), update);

                sentryClient.CaptureMessage($"Game {string.Join(", ", names)} enabled: {enabled}");

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateDisabledMessage(string name, Dictionary<string, string> messages, bool isApplyAll = false)
        {
            try
            {
                var update = Builders<GameConfigEntity>.Update.Set(field => field.DisabledMessage, messages[Language.DefaultLanguageValue]);
                var notificationUpdate = new UpdateRequest
                {
                    DisplayKind = NotificationDisplayKind.DisabledGame,
                    Messages = messages,
                    GameName = name
                };

                if (string.IsNullOrWhiteSpace(name))
                {
                    await repository.UpdateManyAsync(config => true, update);

                    var games = await Get();
                    foreach (var game in games)
                    {
                        notificationUpdate.GameName = game.Info.Name;
                        await notificationService.AddOrUpdateForGame(notificationUpdate);
                    }
                }
                else
                {
                    await repository.UpdateAsync(config => config.Name == name, update);
                    await notificationService.AddOrUpdateForGame(notificationUpdate);
                }

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateBetLimit(int gameId, BetLimit betLimit, IEnumerable<int> otherGameIdsInGroup)
        {
            try
            {
                var gameIds = new List<int>(otherGameIdsInGroup) { gameId };
                var gameEntities = await repository.FilterByAsync(x => gameIds.Contains(x.GameId));
                var writes = new List<UpdateOneModel<GameConfigEntity>>();

                foreach (var gameEntity in gameEntities)
                {
                    var changedSettings = SettingHelper.GetBetLimitChangedModel(gameEntity, betLimit);
                    await chatBotService.NotifyAdminUpdatedSettings(gameEntity.Name, "Bot Setting", string.Empty, changedSettings);

                    var update = Builders<GameConfigEntity>.Update
                        .Set(field => field.Minbet, betLimit.MinBet)
                        .Set(field => field.Maxbet, betLimit.MaxBet)
                        .Set(field => field.MaxBetChoices, betLimit.MaxBetChoices);

                    var filter = Builders<GameConfigEntity>.Filter.Eq(x => x.GameId, gameEntity.GameId);
                    writes.Add(new UpdateOneModel<GameConfigEntity>(filter, update));
                }

                await repository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateOdds(string name, Dictionary<string, double> odds, IEnumerable<int> otherGameIdsInGroup)
        {
            try
            {
                if (odds?.Count == 0)
                {
                    throw new InvalidOperationException($"Odds of game {name} are empty.");
                }

                var gameEntities = await repository.FilterByAsync(x => otherGameIdsInGroup.Contains(x.GameId) || x.Name == name);
                var writes = new List<UpdateOneModel<GameConfigEntity>>();

                foreach (var gameEntity in gameEntities)
                {
                    var changedSettings = SettingHelper.GetOddsChangedModel(gameEntity.Odds, odds);
                    await chatBotService.NotifyAdminUpdatedSettings(gameEntity.Name, "Odds", string.Empty, changedSettings);
                    var update = Builders<GameConfigEntity>.Update.Set(field => field.Odds, odds);
                    var filterDefinition = Builders<GameConfigEntity>.Filter.Eq(x => x.GameId, gameEntity.GameId); ;
                    writes.Add(new UpdateOneModel<GameConfigEntity>(filterDefinition, update));
                }

                await repository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });
                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateMarginSettings(string name, IEnumerable<Dictionary<string, double>> marginSettings)
        {
            try
            {
                if (marginSettings?.Count() == 0)
                {
                    throw new InvalidOperationException($"margins of game {name} are empty.");
                }

                var sortedMarginSettings = marginSettings.OrderBy(setting => setting["max"]);
                var gameEntity = await repository.FindOneAsync(entity => entity.Name == name);
                var changedSettings = SettingHelper.GetMarginSettingsChangedModel(gameEntity.Margin, sortedMarginSettings);
                await chatBotService.NotifyAdminUpdatedSettings(name, "Margin", string.Empty, changedSettings);

                var update = Builders<GameConfigEntity>.Update.Set(field => field.Margin, sortedMarginSettings);
                await repository.UpdateAsync(config => config.Name == name, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateBotRatios(string name, double[] ratios, IEnumerable<int> otherGameIdsInGroup)
        {
            try
            {
                if (ratios.Length < HoursOfDay)
                {
                    throw new InvalidOperationException($"{nameof(ratios)} not fullfill {HoursOfDay} hours.");
                }

                var gameEntities = await repository.FilterByAsync(x => otherGameIdsInGroup.Contains(x.GameId) || x.Name == name);
                var gameIds = gameEntities.Select(x => x.GameId);
                var writes = new List<UpdateOneModel<GameConfigEntity>>();

                foreach (var gameEntity in gameEntities)
                {
                    var changedSettings = SettingHelper.GetBotRatiosChangedModel(gameEntity.HoursMaxBot.Any() ? gameEntity.HoursMaxBot : new double[24], ratios);
                    await chatBotService.NotifyAdminUpdatedSettings(gameEntity.Name, "Bot Ratios", " (GMT-4)", changedSettings);
                    var update = Builders<GameConfigEntity>.Update.Set(field => field.HoursMaxBot, ratios);
                    var filterDefinition = Builders<GameConfigEntity>.Filter.Eq(x => x.GameId, gameEntity.GameId); ;
                    writes.Add(new UpdateOneModel<GameConfigEntity>(filterDefinition, update));
                }

                await repository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, double>> iconOdds, IEnumerable<int> otherGameIdsInGroup)
        {
            try
            {
                if (iconOdds?.Count == 0)
                {
                    throw new InvalidOperationException($"Odds of game {name} are empty.");
                }

                var gameEntities = await repository.FilterByAsync(x => (otherGameIdsInGroup != null && otherGameIdsInGroup.Contains(x.GameId)) || x.Name == name);
                var writes = new List<UpdateOneModel<GameConfigEntity>>();

                foreach (var gameEntity in gameEntities)
                {
                    var changedSettings = SettingHelper.GetIconsChangedModel(gameEntity.SlotsConfig.Icons, iconOdds);
                    await chatBotService.NotifyAdminUpdatedSettings(gameEntity.Name, "Odds", string.Empty, changedSettings);

                    var update = Builders<GameConfigEntity>.Update.Set(field => field.SlotsConfig.Icons, iconOdds);

                    var filter = Builders<GameConfigEntity>.Filter.Eq(x => x.GameId, gameEntity.GameId);
                    writes.Add(new UpdateOneModel<GameConfigEntity>(filter, update));
                }

                await repository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateCascadingSlotOdds(string name, CascadingSlotsConfig cascadingSlotsConfig, IEnumerable<int> otherGameIdsInGroup)
        {
            try
            {
                if (cascadingSlotsConfig.OddsByCounts?.Count == 0)
                {
                    throw new InvalidOperationException($"Odds of game {name} are empty.");
                }

                var gameEntities = await repository.FilterByAsync(x => (otherGameIdsInGroup != null && otherGameIdsInGroup.Contains(x.GameId)) || x.Name == name);
                var writes = new List<UpdateOneModel<GameConfigEntity>>();

                foreach (var gameEntity in gameEntities)
                {
                    var changedSettings = SettingHelper.GetCascadingSlotOddsChangedModel(gameEntity.CascadingSlotsConfig, cascadingSlotsConfig);
                    await chatBotService.NotifyAdminUpdatedSettings(gameEntity.Name, "Cascading Slot Odds Config", string.Empty, changedSettings);

                    var update = Builders<GameConfigEntity>.Update
                        .Set(field => field.CascadingSlotsConfig.OddsByCounts, cascadingSlotsConfig.OddsByCounts)
                        .Set(field => field.CascadingSlotsConfig.FreeSpins, cascadingSlotsConfig.FreeSpins);

                    var filter = Builders<GameConfigEntity>.Filter.Eq(x => x.GameId, gameEntity.GameId);
                    writes.Add(new UpdateOneModel<GameConfigEntity>(filter, update));
                }

                await repository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateJackpot(string name, Jackpot jackpot, List<StakeLevel> stakeLevels, IEnumerable<int> otherGameIdsInGroup)
        {
            try
            {
                var gameEntities = await repository.FilterByAsync(x => (otherGameIdsInGroup != null && otherGameIdsInGroup.Contains(x.GameId)) || x.Name == name);
                var writes = new List<UpdateOneModel<GameConfigEntity>>();

                foreach (var gameEntity in gameEntities)
                {
                    var changedSettings = SettingHelper.GetJackpotChangedModel(gameEntity, jackpot);
                    await chatBotService.NotifyAdminUpdatedSettings(gameEntity.Name, "Jackpot", string.Empty, changedSettings);
                    var filter = Builders<GameConfigEntity>.Filter.Eq(x => x.GameId, gameEntity.GameId);

                    if (jackpot.Pools != null)
                    {
                        var update = Builders<GameConfigEntity>.Update
                        .Set(field => field.SlotsConfig.StakeLevels, stakeLevels)
                        .Set(field => field.SlotsConfig.Jackpot.ExcludeTestCurrency, jackpot.ExcludeTestCurrency)
                        .Set(field => field.SlotsConfig.Jackpot.WinningRatio, jackpot.WinningRatio)
                        .Set(field => field.SlotsConfig.Jackpot.Pools.Minor.Init, jackpot.Pools?.Minor?.Init)
                        .Set(field => field.SlotsConfig.Jackpot.Pools.Major.Init, jackpot.Pools?.Major?.Init)
                        .Set(field => field.SlotsConfig.Jackpot.Pools.Grand.Init, jackpot.Pools?.Grand?.Init)
                        .Set(field => field.SlotsConfig.Jackpot.Pools.Minor.PlayerSettings.Contrib, jackpot.Pools?.Minor?.PlayerSettings?.Contrib)
                        .Set(field => field.SlotsConfig.Jackpot.Pools.Major.PlayerSettings.Contrib, jackpot.Pools?.Major?.PlayerSettings?.Contrib)
                        .Set(field => field.SlotsConfig.Jackpot.Pools.Grand.PlayerSettings.Contrib, jackpot.Pools?.Grand?.PlayerSettings?.Contrib);

                        writes.Add(new UpdateOneModel<GameConfigEntity>(filter, update));
                    }
                    else
                    {
                        var update = Builders<GameConfigEntity>.Update
                           .Set(field => field.SlotsConfig.StakeLevels, stakeLevels)
                           .Set(field => field.SlotsConfig.Jackpot.ExcludeTestCurrency, jackpot.ExcludeTestCurrency)
                           .Set(field => field.SlotsConfig.Jackpot.WinningRatio, jackpot.WinningRatio)
                           .Set(field => field.SlotsConfig.Jackpot.BotContributionRatio, jackpot.BotContributionRatio)
                           .Set(field => field.SlotsConfig.Jackpot.PlayerContributionRatio, jackpot.PlayerContributionRatio);

                        writes.Add(new UpdateOneModel<GameConfigEntity>(filter, update));
                    }
                }

                await repository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateSlotBotSettings(string name, Jackpot jackpot, double[] betLevels, IEnumerable<int> otherGameIdsInGroup)
        {
            try
            {
                var gameEntities = await repository.FilterByAsync(x => (otherGameIdsInGroup != null && otherGameIdsInGroup.Contains(x.GameId)) || x.Name == name);
                var writes = new List<UpdateOneModel<GameConfigEntity>>();

                foreach (var gameEntity in gameEntities)
                {
                    var changedSettings = SettingHelper.GetBotConfigChangedModel(gameEntity, jackpot, betLevels);
                    await chatBotService.NotifyAdminUpdatedSettings(gameEntity.Name, "Jackpot", string.Empty, changedSettings);
                    var sortBetLevel = betLevels.OrderBy(level => level).ToArray();

                    var update = Builders<GameConfigEntity>.Update
                        .Set(field => field.SlotsConfig.BetLevels, sortBetLevel)
                        .Set(field => field.SlotsConfig.Jackpot.Pools.Minor.BotSettings, jackpot.Pools?.Minor?.BotSettings)
                        .Set(field => field.SlotsConfig.Jackpot.Pools.Major.BotSettings, jackpot.Pools?.Major?.BotSettings)
                        .Set(field => field.SlotsConfig.Jackpot.Pools.Grand.BotSettings, jackpot.Pools?.Grand?.BotSettings)
                        .Set(field => field.SlotsConfig.Jackpot.Pools.Minor.PlayerSettings.Alloc, jackpot.Pools?.Minor?.PlayerSettings?.Alloc)
                        .Set(field => field.SlotsConfig.Jackpot.Pools.Major.PlayerSettings.Alloc, jackpot.Pools?.Major?.PlayerSettings?.Alloc)
                        .Set(field => field.SlotsConfig.Jackpot.Pools.Grand.PlayerSettings.Alloc, jackpot.Pools?.Grand?.PlayerSettings?.Alloc);

                    var filter = Builders<GameConfigEntity>.Filter.Eq(x => x.GameId, gameEntity.GameId);
                    writes.Add(new UpdateOneModel<GameConfigEntity>(filter, update));
                }

                await repository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateSlotConfig(string name, bool botEnabled, bool enablePromotionRTP, SlotsConfig slotsConfig, IEnumerable<int> otherGameIdsInGroup)
        {
            try
            {
                var gameIds = new List<int>(otherGameIdsInGroup ?? new List<int>()) { };
                var gameEntities = await repository.FilterByAsync(x => otherGameIdsInGroup.Contains(x.GameId) || x.Name == name);

                var writes = new List<UpdateOneModel<GameConfigEntity>>();

                foreach (var gameEntity in gameEntities)
                {
                    var changedSettings = SettingHelper.GetSlotConfigChangedModel(gameEntity, botEnabled, enablePromotionRTP, slotsConfig);
                    await chatBotService.NotifyAdminUpdatedSettings(gameEntity.Name, "Slot Config", string.Empty, changedSettings);
                    var update = Builders<GameConfigEntity>.Update
                            .Set(field => field.Botenabled, botEnabled)
                            .Set(field => field.SlotsConfig.Bonus, slotsConfig.Bonus)
                            .Set(field => field.SlotsConfig.BigWinRatio, slotsConfig.BigWinRatio)
                            .Set(field => field.SlotsConfig.Mode, slotsConfig.Mode)
                            .Set(field => field.EnablePromotionRTP, enablePromotionRTP);
                    var filterDefinition = Builders<GameConfigEntity>.Filter.Eq(x => x.GameId, gameEntity.GameId); ;
                    writes.Add(new UpdateOneModel<GameConfigEntity>(filterDefinition, update));
                }

                await repository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateCascadingSlotsConfig(string name, bool botEnabled, bool enablePromotionRTP, CascadingSlotsConfig cascadingSlotsConfig, IEnumerable<int> otherGameIdsInGroup)
        {
            try
            {
                var gameIds = new List<int>(otherGameIdsInGroup ?? new List<int>()) { };
                var gameEntities = await repository.FilterByAsync(x => otherGameIdsInGroup.Contains(x.GameId) || x.Name == name);

                var writes = new List<UpdateOneModel<GameConfigEntity>>();

                foreach (var gameEntity in gameEntities)
                {
                    var changedSettings = SettingHelper.GetCascadingSlotConfigsChangeModel(gameEntity, botEnabled, enablePromotionRTP, cascadingSlotsConfig);
                    await chatBotService.NotifyAdminUpdatedSettings(gameEntity.Name, "Cascading Slot Config", string.Empty, changedSettings);
                    var update = Builders<GameConfigEntity>.Update
                       .Set(field => field.Botenabled, botEnabled)
                       .Set(field => field.EnablePromotionRTP, enablePromotionRTP)
                       .Set(field => field.CascadingSlotsConfig.MultiplierBonus, cascadingSlotsConfig.MultiplierBonus)
                       .Set(field => field.CascadingSlotsConfig.BigWinRatio, cascadingSlotsConfig.BigWinRatio)
                       .Set(field => field.CascadingSlotsConfig.MegaWinRatio, cascadingSlotsConfig.MegaWinRatio)
                       .Set(field => field.CascadingSlotsConfig.SuperWinRatio, cascadingSlotsConfig.SuperWinRatio)
                       .Set(field => field.CascadingSlotsConfig.Mode, cascadingSlotsConfig.Mode)
                       .Set(field => field.CascadingSlotsConfig.Volatility, cascadingSlotsConfig.Volatility)
                       .Set(field => field.CascadingSlotsConfig.BetMultiplier, cascadingSlotsConfig.BetMultiplier)
                       .Set(field => field.CascadingSlotsConfig.BetLevels, cascadingSlotsConfig.BetLevels)
                       .Set(field => field.CascadingSlotsConfig.ResultChains, cascadingSlotsConfig.ResultChains);
                    var filterDefinition = Builders<GameConfigEntity>.Filter.Eq(x => x.GameId, gameEntity.GameId);
                    writes.Add(new UpdateOneModel<GameConfigEntity>(filterDefinition, update));
                }

                await repository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateGeneralConfig(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, OddsMultipliersConfig oddsMultipliers = null, IEnumerable<int> otherGameIdsInGroup = null, TournamentConfig tournamentConfig = null, VideoStreamConfig videoStreamConfig = null)
        {
            try
            {
                var gameIds = new List<int>(otherGameIdsInGroup ?? new List<int>()) { gameInfo.GameId };
                var gameEntities = await repository.FilterByAsync(entity => gameIds.Contains(entity.GameId));
                var writes = new List<UpdateOneModel<GameConfigEntity>>();
                foreach (var gameEntity in gameEntities)
                {
                    var changedSettings = SettingHelper.GetGeneralConfigChangedModel(gameEntity, gameInfo, botSetting, enableSetting, oddsMultipliers, tournamentConfig, videoStreamConfig);
                    await chatBotService.NotifyAdminUpdatedSettings(gameEntity.Name, "General Config", string.Empty, changedSettings);

                    var update = Builders<GameConfigEntity>.Update
                        .Set(field => field.EnableFreeBet, gameInfo.EnableFreeBet)
                        .Set(field => field.DelayStartTime, enableSetting.DelayStartTime)
                        .Set(field => field.Botenabled, botSetting.BotEnabled)
                        .Set(field => field.BotMaxBet, botSetting.BotMaxBet)
                        .Set(field => field.SmartBotEnabled, botSetting.SmartBotEnabled)
                        .Set(field => field.SmartBotGroupRatio, botSetting.SmartBotGroupRatio)
                        .Set(field => field.RandomMode, gameInfo.RandomMode)
                        .Set(field => field.MaxWinPerRound, gameInfo.MaxWinPerRound)
                        .Set(field => field.OddsMultipliers, oddsMultipliers)
                        .Set(field => field.TournamentConfig, tournamentConfig)
                        .Set(field => field.VideoStreamConfig, videoStreamConfig);

                    var filter = Builders<GameConfigEntity>.Filter.Eq(x => x.GameId, gameEntity.GameId);

                    writes.Add(new UpdateOneModel<GameConfigEntity>(filter, update));
                }

                await repository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<Dictionary<string, string>> GetDisabledMessage(string name)
        {
            var notification = await notificationService.GetByGameName(name, NotificationDisplayKind.DisabledGame);

            if (notification != null)
            {
                return notification.Messages;
            }

            var gameConfig = await GetByName(name);

            return new Dictionary<string, string> { { Language.DefaultLanguageValue, gameConfig.EnableSetting.DisabledMessage } };
        }

        public async Task<bool> UpdateSlotRNGConfig(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, SlotRNGConfig slotRNGConfig)
        {
            var gameEntity = await repository.FindOneAsync(entity => entity.Name == gameInfo.Name);
            var changedSettings = SettingHelper.GetSlotRNGConfigsChangeModel(gameEntity, gameInfo, botSetting, enableSetting, slotRNGConfig);
            await chatBotService.NotifyAdminUpdatedSettings(gameInfo.Name, "General Config", string.Empty, changedSettings);

            var update = Builders<GameConfigEntity>.Update
                .Set(field => field.EnableFreeBet, gameInfo.EnableFreeBet)
                .Set(field => field.DelayStartTime, enableSetting.DelayStartTime)
                .Set(field => field.SlotRNGConfig.Mode, slotRNGConfig.Mode)
                .Set(field => field.SlotRNGConfig.LuckySpinFrom, slotRNGConfig.LuckySpinFrom)
                .Set(field => field.SlotRNGConfig.LuckySpinTo, slotRNGConfig.LuckySpinTo)
                .Set(field => field.Botenabled, botSetting.BotEnabled)
                .Set(field => field.BotMaxBet, botSetting.BotMaxBet)
                .Set(field => field.SmartBotEnabled, botSetting.SmartBotEnabled)
                .Set(field => field.SmartBotGroupRatio, botSetting.SmartBotGroupRatio)
                .Set(field => field.RandomMode, gameInfo.RandomMode);

            await repository.UpdateAsync(config => config.Name == gameInfo.Name, update);

            return true;
        }

        public async Task<bool> UpdateSymbolWeight(string name, SymbolWeight symbolWeight, IEnumerable<int> otherGameIdsInGroup)
        {
            if (string.IsNullOrEmpty(name) || symbolWeight.Weight == null || symbolWeight.Weight.Length < 3)
            {
                return false;
            }

            try
            {
                var gameIds = new List<int>(otherGameIdsInGroup ?? new List<int>()) { };
                var gameEntities = await repository.FilterByAsync(x => gameIds.Contains(x.GameId) || x.Name == name);

                var writes = new List<UpdateOneModel<GameConfigEntity>>();

                foreach (var gameEntity in gameEntities)
                {
                    var existingSlotSymbolWeight = gameEntity.SlotsConfig.SymbolWeight ?? new Dictionary<string, int[][]>();

                    await chatBotService.NotifyAdminUpdatedSettings(gameEntity.Name, "Symbol Weight", string.Empty, SettingHelper.GetSymbolWeightChangedModel(existingSlotSymbolWeight, symbolWeight));

                    var keyMode = symbolWeight.Mode.ToString();

                    var slotSymbolWeight = GetUpdatedSlotSymbolWeight(existingSlotSymbolWeight, keyMode, symbolWeight.Weight);
                    var update = Builders<GameConfigEntity>.Update.Set(field => field.SlotsConfig.SymbolWeight, slotSymbolWeight);
                    var filterDefinition = Builders<GameConfigEntity>.Filter.Eq(x => x.GameId, gameEntity.GameId); ;
                    writes.Add(new UpdateOneModel<GameConfigEntity>(filterDefinition, update));
                }

                await repository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        private static Dictionary<string, int[][]> GetUpdatedSlotSymbolWeight(Dictionary<string, int[][]> slotSymbolWeight, string keyMode, int[][] weights)
        {
            if (!slotSymbolWeight.ContainsKey(keyMode))
            {
                slotSymbolWeight.Add(keyMode, weights);
            }
            else
            {
                slotSymbolWeight[keyMode] = weights;
            }

            return slotSymbolWeight;
        }

        public async Task<bool> UpdateCrashConfig(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, CrashConfig crashConfig)
        {
            var gameEntity = await repository.FindOneAsync(entity => entity.Name == gameInfo.Name);
            var changedSettings = SettingHelper.GetCrashConfigChangeModel(gameEntity, gameInfo, botSetting, enableSetting, crashConfig);
            await chatBotService.NotifyAdminUpdatedSettings(gameInfo.Name, "General Config", string.Empty, changedSettings);

            var update = Builders<GameConfigEntity>.Update
                .Set(field => field.EnableFreeBet, gameInfo.EnableFreeBet)
                .Set(field => field.DelayStartTime, enableSetting.DelayStartTime)
                .Set(field => field.CrashConfig.MaxMultiplier, crashConfig.MaxMultiplier)
                .Set(field => field.CrashConfig.MaxWinPerRound, crashConfig.MaxWinPerRound)
                .Set(field => field.CrashConfig.CrashAtBase, crashConfig.CrashAtBase)
                .Set(field => field.Botenabled, botSetting.BotEnabled)
                .Set(field => field.BotMaxBet, botSetting.BotMaxBet)
                .Set(field => field.SmartBotEnabled, botSetting.SmartBotEnabled)
                .Set(field => field.SmartBotGroupRatio, botSetting.SmartBotGroupRatio)
                .Set(field => field.RandomMode, gameInfo.RandomMode)
                .Set(field => field.CrashConfig.MultiplierConfigs, crashConfig.MultiplierConfigs);

            await repository.UpdateAsync(config => config.Name == gameInfo.Name, update);

            return true;
        }

        public async Task<bool> UpdatePlinkoConfig(string name, PlinkoConfig plinkoConfig)
        {
            try
            {
                if (plinkoConfig.Odds?.Count == 0)
                {
                    throw new InvalidOperationException($"Odds of game {name} are empty.");
                }

                var gameEntities = await repository.FilterByAsync(x => x.Name == name);
                var writes = new List<UpdateOneModel<GameConfigEntity>>();

                foreach (var gameEntity in gameEntities)
                {
                    var changedSettings = SettingHelper.GetPlinkoConfigChangedModel(gameEntity.PlinkoConfig, plinkoConfig);
                    await chatBotService.NotifyAdminUpdatedSettings(gameEntity.Name, "Plinko Odds Config", string.Empty, changedSettings);

                    var update = Builders<GameConfigEntity>.Update
                        .Set(field => field.PlinkoConfig.Odds, plinkoConfig.Odds);

                    var filter = Builders<GameConfigEntity>.Filter.Eq(x => x.GameId, gameEntity.GameId);
                    writes.Add(new UpdateOneModel<GameConfigEntity>(filter, update));
                }

                await repository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> IsGameIdExists(int gameId)
        {
            try
            {
                var existingGame = await Task.Run(() => repository.AsQueryable()
                    .FirstOrDefault(g => g.GameId == gameId));

                return existingGame != null;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return false;
            }
        }

        public async Task<bool> IsGameNameExists(string gameName)
        {
            try
            {
                var existingGame = await Task.Run(() => repository.AsQueryable()
                    .FirstOrDefault(g => g.Name == gameName));

                return existingGame != null;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return false;
            }
        }

        /// <summary>
        /// Sets additional game configuration properties on the entity from the game config
        /// This method can be extended to handle other properties that need special assignment logic
        /// </summary>
        /// <param name="entity">The game config entity to update</param>
        /// <param name="gameConfig">The game configuration to get properties from</param>
        private static void SetAdditionalGameProperties(GameConfigEntity entity, GameConfig gameConfig)
        {
            entity.PlinkoConfig = gameConfig.PlinkoConfig;
            // Add other properties here as needed in the future
        }

        /// <summary>
        /// Notifies administrators about a new game creation via ChatBot service
        /// </summary>
        /// <param name="gameName">The name of the newly created game</param>
        /// <param name="gameEntity">The game entity that was created</param>
        private async Task NotifyNewGameCreated(string gameName, GameConfigEntity gameEntity)
        {
            try
            {
                // Send notification asynchronously to avoid blocking
                await chatBotService.NotifyAdminAddSettings(
                    "ADD",
                    $"Game {gameName}",
                    gameEntity);
            }
            catch (Exception ex)
            {
                // Log the exception but don't throw to avoid breaking the main flow
                sentryClient.CaptureException(ex);
            }
        }
    }
}