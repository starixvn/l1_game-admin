using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Api.GameUser;
using GamesAdmin.Api.UM.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Microsoft.IdentityModel.Tokens;
using Refit;
using Sentry;
using HiloCard = GamesAdmin.Core.Models.Card;

namespace GamesAdmin.Api.GameServers
{
    public interface IGameServerService
    {
        Task UnderMaintenance(ServerApiUMRequest request);

        Task Reload();

        Task<JsonWebToken> GetJWT(JWTRequest request);

        Task<OnlineUsers> GetOnlineUsers(Game gameType = null);

        Task<bool> NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds);

        Task<SettlementBoard> SetSlotBoard(SetSlotBoardRequest request);

        Task<HoldemPokerResultControl> SetHoldemPokerResult(SetHoldemPokerResultRequest request);

        Task<IEnumerable<HiloCard>> SetHiLoResult(SetHiLoResultRequest request);

        Task<AdminMonitoringModel> GetAdminMonitoringSettings(string username);

        Task<bool> VerifyAdminMonitoringToken(string token);
    }

    public class GameServerService : IGameServerService
    {
        private readonly IGameServerApi serverApi;
        private readonly ISentryClient sentryClient;
        private readonly IOnlineUserApiFactory onlineUserApiFactory;
        private readonly ISlotBoardApiFactory slotBoardApiFactory;
        private readonly IGameUserService gameUserService;
        private readonly IGameSettingsService gameSettingService;
        private readonly IHoldemPokerApiFactory holdemPokerApiFactory;
        private readonly IHiLoApi hiloApi;
        private readonly IGenericRepository<AdminUserEntity> userRepository;
        private readonly IAppSettings appSettings;

        public GameServerService(
            IGameServerApi serverApi,
            ISentryClient sentryClient,
            IOnlineUserApiFactory onlineUserApiFactory,
            ISlotBoardApiFactory slotBoardApiFactory,
            IGameUserService gameUserService,
            IGameSettingsService gameSettingService,
            IHoldemPokerApiFactory holdemPokerApiFactory,
            IHiLoApi hiloApi,
            IAppSettings appSettings,
            IGenericRepository<AdminUserEntity> userRepository)
        {
            this.serverApi = serverApi;
            this.sentryClient = sentryClient;
            this.onlineUserApiFactory = onlineUserApiFactory;
            this.slotBoardApiFactory = slotBoardApiFactory;
            this.gameUserService = gameUserService;
            this.gameSettingService = gameSettingService;
            this.holdemPokerApiFactory = holdemPokerApiFactory;
            this.hiloApi = hiloApi;
            this.appSettings = appSettings;
            this.userRepository = userRepository;
        }

        public Task<JsonWebToken> GetJWT(JWTRequest request)
        {
            try
            {
                return serverApi.GetToken(request);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return Task.FromResult(new JsonWebToken());
        }

        public async Task Reload()
        {
            try
            {
                await serverApi.Reload();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public async Task UnderMaintenance(ServerApiUMRequest request)
        {
            try
            {
                await serverApi.UnderMaintenance(request);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public async Task<OnlineUsers> GetOnlineUsers(Game gameType = null)
        {
            try
            {
                if (gameType == null)
                {
                    return await serverApi.GetAllOnlineUsers();
                }

                var onlineUserService = onlineUserApiFactory.GetOnlineUserApi(gameType);

                return await onlineUserService.GetOnlineUsers(ConvertTypeToGameServerName(gameType));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return new OnlineUsers();
        }

        private static string ConvertTypeToGameServerName(Game gameType)
        {
            return gameType.Value switch
            {
                Game.KenoProMaxValue => "keno",
                Game.BaccaratValue => "baccarat",
                Game.RouletteValue => Game.RouletteValue,
                Game.KenoMatch1Value => Game.KenoMatch1Value,
                Game.KenoMatch2Value => Game.KenoMatch2Value,
                Game.KenoPower1Value => Game.KenoPower1Value,
                Game.KenoPower2Value => Game.KenoPower2Value,
                _ => string.Empty,
            };
        }

        private void HandleException(Exception ex, string title = null)
        {
            if (ex is ApiException apiEx)
            {
                sentryClient.CaptureMessage($"Request {apiEx.Uri} error: {apiEx.Message}");
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                sentryClient.CaptureMessage(title, Sentry.Protocol.SentryLevel.Error);
            }

            sentryClient.CaptureException(ex);
        }

        public async Task<bool> NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds)
        {
            foreach (var round in kenoRounds)
            {
                try
                {
                    sentryClient.CaptureMessage($"NotifyRoundResult {round.Number} at {DateTime.Now}");

                    await serverApi.NotifyRoundResult(round);
                }
                catch (Exception ex)
                {
                    HandleException(ex, $"{DateTime.Now} - NotifyRoundResult failed for round #{round.Number}.");
                }
            }

            return true;
        }

        public async Task<SettlementBoard> SetSlotBoard(SetSlotBoardRequest request)
        {
            try
            {
                var config = await gameSettingService.GetByName(request.GameConfigName);
                var isExcludeTestCurrency = (config?.SlotsConfig?.Jackpot) != null && config.SlotsConfig.Jackpot.ExcludeTestCurrency;

                if (await gameUserService.IsAbleToSetSlotResult(request.Nickname, isExcludeTestCurrency))
                {
                    var gameEnum = Enumeration.FromValue<Game>(request.GameConfigName);

                    var result = await slotBoardApiFactory.SetBoardApi(gameEnum).SetBoard(new SetSlotBoardServerRequest(request.Nickname, request.Results));
                    result.Result = request.Results.ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public async Task<HoldemPokerResultControl> SetHoldemPokerResult(SetHoldemPokerResultRequest request)
        {
            try
            {
                var pokerApi = holdemPokerApiFactory.SetResultApi(Enumeration.FromValue<Game>(request.gameConfigName));
                var model = await pokerApi.SetResult(request);

                return new HoldemPokerResultControl
                {
                    playerResult = new HoldemPokerHandModel { Cards = request.player, CombinationCards = model.playerResult.Cards, Id = model.playerResult.Id },
                    dealerResult = new HoldemPokerHandModel { Cards = request.dealer, CombinationCards = model.dealerResult.Cards, Id = model.dealerResult.Id },
                    communityCards = request.communityCards,
                    isQualified = model.isQualified
                };
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        public async Task<IEnumerable<HiloCard>> SetHiLoResult(SetHiLoResultRequest request)
        {
            try
            {
                return await hiloApi.SetResult(request);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return Enumerable.Empty<HiloCard>();
        }

        public Task<AdminMonitoringModel> GetAdminMonitoringSettings(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.AdminMonitoringSettings.JwtSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("username", username.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(appSettings.AdminMonitoringSettings.ExpiredTokenByMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(new AdminMonitoringModel(tokenHandler.WriteToken(token), appSettings.AdminMonitoringSettings.Url));
        }

        public async Task<bool> VerifyAdminMonitoringToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.AdminMonitoringSettings.JwtSecretKey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = jwtToken.Claims.First(x => x.Type == "username").Value.ToString();

                var userEntity = await userRepository.FindOneAsync(entity => entity.Username.Equals(username));

                return userEntity != null;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }
    }
}