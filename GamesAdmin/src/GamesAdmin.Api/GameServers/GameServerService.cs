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
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.IdentityServer;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.FootballDice;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Microsoft.IdentityModel.Tokens;
using Refit;
using Sentry;
using HiloCard = GamesAdmin.Core.Models.Card;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.GameServers
{
    public interface IGameServerService
    {
        Task UnderMaintenance(ServerApiUMRequest request);

        Task Reload();

        Task<JsonWebToken> GetJWT(JWTRequest request);

        Task<OnlineUsers> GetOnlineUsers(string gameConfigName = null);

        Task<HoldemPokerResultControl> SetHoldemPokerResult(SetHoldemPokerResultRequest request);

        Task<IEnumerable<HiloCard>> SetHiLoResult(SetHiLoResultRequest request);

        Task<AdminMonitoringModel> GetAdminMonitoringSettings(string username);

        Task<IEnumerable<SettlementCascadingSlotResult>> SetCascadingSlotResult(SetCascadingSlotResultRequest request);

        Task<bool> VerifyAdminMonitoringToken(string token);

        Task<int> GetConnectedUsers();

        Task<SettlementBoard> SetSlotBoard(SetSlotBoardRequest request);

        Task<bool> SetPontoonResult(SetPontoonResultRequest request);

        Task<bool> SetChineseBlackjackResult(SetChineseBlackjackResultRequest request);

        Task<bool> SetCrashGameResult(SetCrashGameResultRequest request);

        Task<FootballDiceControlResult> SetFootballDiceResult(SetFootballDiceResultRequest request);

        Task<bool> SetBaccaratXRersult(SetBaccaratXRersultRequest request);
    }

    public class GameServerService : IGameServerService
    {
        private readonly IUserServiceApi userServiceApi;
        private readonly INotificationServiceApi notificationServiceApi;
        private readonly ISentryClient sentryClient;
        private readonly IOnlineUserApiFactory onlineUserApiFactory;
        private readonly ISlotBoardApiFactory slotBoardApiFactory;
        private readonly IGameUserService gameUserService;
        private readonly IGameSettingsService gameSettingService;
        private readonly IHoldemPokerApiFactory holdemPokerApiFactory;
        private readonly IHiLoApiFactory hiloApiFactory;
        private readonly IGenericRepository<AdminUserEntity> userRepository;
        private readonly ICascadingResultApiFactory cascadingResultApiFactory;
        private readonly IAppSettings appSettings;
        private readonly IPontoonApiFactory pontoonApiFactory;
        private readonly IChineseBlackjackApiFactory chineseBlackjackApiFactory;
        private readonly ICrashGameApiFactory crashGameApiFactory;
        private readonly IFootballDiceApiFactory footballDiceApiFactory;
        private readonly IServiceProvider serviceProvider;

        public GameServerService(
            ISentryClient sentryClient,
            IOnlineUserApiFactory onlineUserApiFactory,
            ISlotBoardApiFactory slotBoardApiFactory,
            IGameUserService gameUserService,
            IGameSettingsService gameSettingService,
            IHoldemPokerApiFactory holdemPokerApiFactory,
            IHiLoApiFactory hiloApiFactory,
            IAppSettings appSettings,
            IUserServiceApi userServiceApi,
            ICascadingResultApiFactory cascadingResultApiFactory,
            INotificationServiceApi notificationServiceApi,
            IGenericRepository<AdminUserEntity> userRepository,
            IPontoonApiFactory pontoonApiFactory,
            IChineseBlackjackApiFactory chineseBlackjackApiFactory,
            ICrashGameApiFactory crashGameApiFactory,
            IFootballDiceApiFactory footballDiceApiFactory,
            IServiceProvider serviceProvider)
        {
            this.sentryClient = sentryClient;
            this.onlineUserApiFactory = onlineUserApiFactory;
            this.slotBoardApiFactory = slotBoardApiFactory;
            this.gameUserService = gameUserService;
            this.gameSettingService = gameSettingService;
            this.holdemPokerApiFactory = holdemPokerApiFactory;
            this.hiloApiFactory = hiloApiFactory;
            this.appSettings = appSettings;
            this.userServiceApi = userServiceApi;
            this.cascadingResultApiFactory = cascadingResultApiFactory;
            this.notificationServiceApi = notificationServiceApi;
            this.userRepository = userRepository;
            this.pontoonApiFactory = pontoonApiFactory;
            this.chineseBlackjackApiFactory = chineseBlackjackApiFactory;
            this.crashGameApiFactory = crashGameApiFactory;
            this.footballDiceApiFactory = footballDiceApiFactory;
            this.serviceProvider = serviceProvider;
        }

        public Task<JsonWebToken> GetJWT(JWTRequest request)
        {
            try
            {
                return userServiceApi.GetToken(request);
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
                await notificationServiceApi.Reload();
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
                await notificationServiceApi.UnderMaintenance(request);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public async Task<OnlineUsers> GetOnlineUsers(string gameConfigName = null)
        {
            try
            {
                if (gameConfigName == null)
                {
                    return await userServiceApi.GetAllOnlineUsers();
                }

                var gameConfig = await gameSettingService.GetByName(gameConfigName);

                var onlineUserService = onlineUserApiFactory.GetOnlineUserApi(gameConfig);

                var header = await IdentityServerManager.GetAuthorizationHeader();
                return await onlineUserService.GetOnlineUsers(ConvertTypeToGameServerName(gameConfigName), header);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return new OnlineUsers();
        }

        private static string ConvertTypeToGameServerName(string gameType)
        {
            return gameType switch
            {
                Game.KenoProMaxValue => "keno",
                _ => gameType,
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

        public async Task<SettlementBoard> SetSlotBoard(SetSlotBoardRequest request)
        {
            try
            {
                var config = await gameSettingService.GetByName(request.GameConfigName);
                var isExcludeTestCurrency = (config?.SlotsConfig?.Jackpot) != null && config.SlotsConfig.Jackpot.ExcludeTestCurrency;

                if (await gameUserService.IsAbleToSetSlotResult(request.Nickname, isExcludeTestCurrency))
                {
                    var serviceName = NamingConverter.ServiceName(config?.Info.DisplayName, appSettings.ServiceNameSeparator);
                    var result = await slotBoardApiFactory.GetBoardApi(serviceName).SetBoard(new SetSlotBoardServerRequest(request.Nickname, request.Results));
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
                var pokerApi = holdemPokerApiFactory.SetResultApi(Game.FromGameName(request.gameConfigName));
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
                var hiloApi = hiloApiFactory.SetResultApi();
                return await hiloApi.SetResult(request);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return Enumerable.Empty<HiloCard>();
        }

        public async Task<bool> SetPontoonResult(SetPontoonResultRequest request)
        {
            try
            {
                var pontoonApi = pontoonApiFactory.SetResultApi();
                return await pontoonApi.SetResult(request);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
        }

        public async Task<bool> SetChineseBlackjackResult(SetChineseBlackjackResultRequest request)
        {
            try
            {
                var chineseBlackjackApi = chineseBlackjackApiFactory.SetResultApi();
                return await chineseBlackjackApi.SetResult(request);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
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

        public async Task<int> GetConnectedUsers()
        {
            try
            {
                return await notificationServiceApi.ConnectedClients();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return 0;
        }

        public async Task<IEnumerable<SettlementCascadingSlotResult>> SetCascadingSlotResult(SetCascadingSlotResultRequest request)
        {
            try
            {
                var config = await gameSettingService.GetByName(request.gameConfigName);
                if (await gameUserService.IsAbleToSetSlotResult(request.name, false))
                {
                    var serviceName = NamingConverter.ServiceName(config?.Info.DisplayName, appSettings.ServiceNameSeparator);
                    var result = await cascadingResultApiFactory.SetResultApi(serviceName).SetResult(request);

                    return result;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public async Task<bool> SetCrashGameResult(SetCrashGameResultRequest request)
        {
            try
            {
                var config = await gameSettingService.GetByName(request.gameConfigName);

                var serviceName = NamingConverter.ServiceName(config?.Info.DisplayName, appSettings.ServiceNameSeparator);
                return await crashGameApiFactory.SetResultApi(serviceName).SetResult(request);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        public async Task<FootballDiceControlResult> SetFootballDiceResult(SetFootballDiceResultRequest request)
        {
            return await footballDiceApiFactory.SetResultApi().SetResult(request);
        }

        public async Task<bool> SetBaccaratXRersult(SetBaccaratXRersultRequest request)
        {
            try
            {
                var baccaratXApi = serviceProvider.GetRequiredService<IBaccaratXApi>();
                return await baccaratXApi.SetResult(request, request.gameConfigName);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
        }
    }
}