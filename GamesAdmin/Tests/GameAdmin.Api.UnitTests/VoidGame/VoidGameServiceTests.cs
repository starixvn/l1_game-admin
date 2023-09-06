using AutoFixture.Xunit2;
using GamesAdmin.Api.VoidGame;
using GamesAdmin.Api.VoidGame.Models;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Moq;
using NSubstitute;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace GamesAdmin.Api.UnitTests.VoidGame
{

    public class VoidGameServiceTests
    {
        private readonly ISentryClient sentryClient;

        private readonly IVoidGameApi voidGameApi;

        private readonly ITicketHandler ticketHandler;

        private readonly IVoidGameService voidGameService;

        private readonly Mock<IVoidRunningTicketApiFactory> voidRunningTicketApiFactory;

        private readonly Mock<IGenericRepository<AccountInfoEntity>> accountInfoRepository;

        public VoidGameServiceTests()
        {
            sentryClient = Substitute.For<ISentryClient>();
            voidGameApi = Substitute.For<IVoidGameApi>();
            ticketHandler = Substitute.For<ITicketHandler>();
            voidRunningTicketApiFactory = new Mock<IVoidRunningTicketApiFactory>();
            accountInfoRepository = new Mock<IGenericRepository<AccountInfoEntity>>();
            voidGameService = new VoidGameService(sentryClient, voidGameApi, ticketHandler, voidRunningTicketApiFactory.Object, accountInfoRepository.Object);
        }

        [Theory, AutoData]
        public async Task VoidBlackJackTicket_NotExist_ReturnCorrectResult(VoidTicketRequest request)
        {
            //Arrange
            var expected = new VoidGameResult
            {
                ErrorCode = 1001,
                ErrorMessage = "No such ticket."
            };
            var roundIds = new List<long> { 1 };

            var blackjackRecord = new BlackjackRecord(null, false, roundIds);
            ticketHandler.ExistBankingGameTicket<BlackjackBetEntity>(request.PlayerId,
                    request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.FreeBet).Returns(blackjackRecord);

            //Act
            var result = await voidGameService.VoidBankingGameTicket<BlackjackRoundEntity, BlackjackBetEntity>(request);

            //Assert
            Assert.Equal(expected.ErrorCode, result.ErrorCode);
            Assert.Equal(expected.ErrorMessage, result.ErrorMessage);
        }

        [Theory, AutoData]
        public async Task VoidBlackJackTicket_NormalTicket_VoidFailed_ReturnCorrectResult(VoidTicketRequest request)
        {
            //Arrange
            var expected = new VoidGameResult
            {
                ErrorCode = 1000,
                ErrorMessage = "Tickets was moved unsuccessfully.",
            };
            var blackJackEntity = new BlackjackBetEntity
            {
                SiteId = "4100300",
                HandId = 1,
                Round = 34,
                Paid = true
            };
            var roundIds = new List<long> { 1 };
            var blackjackRecord = new BlackjackRecord(blackJackEntity, false, roundIds);
            ticketHandler.ExistBankingGameTicket<BlackjackBetEntity>(request.PlayerId,
                    request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.FreeBet).Returns(blackjackRecord);
            var apiResult = new VoidGameApiResult
            {
                Result = new VoidGameResult
                {
                    ErrorCode = 0,
                }
            };
            voidGameApi.VoidGameTicket(request).Returns(apiResult);

            //Act
            var result = await voidGameService.VoidBankingGameTicket<BlackjackRoundEntity, BlackjackBetEntity>(request);

            //Assert
            Assert.Equal(expected.ErrorCode, result.ErrorCode);
            Assert.Equal(expected.ErrorMessage, result.ErrorMessage);
        }

        [Theory, AutoData]
        public async Task VoidBlackJackTicket_NormalTicket_VoidSuccessfull_ReturnCorrectResult(VoidTicketRequest request)
        {
            //Arrnage
            var blackJackEntity = new BlackjackBetEntity
            {
                SiteId = "4100300",
                HandId = 1,
                Round = 34,
                Paid = true
            };
            var roundIds = new List<long> { 1, 2 };
            var blackjackRecord = new BlackjackRecord(blackJackEntity, false, roundIds);
            ticketHandler.ExistBankingGameTicket<BlackjackBetEntity>(request.PlayerId,
                    request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.FreeBet).Returns(blackjackRecord);
            var apiResult = new VoidGameApiResult
            {
                Result = new VoidGameResult
                {
                    ErrorCode = 0,
                }
            };
            voidGameApi.VoidGameTicket(request).Returns(apiResult);
            ticketHandler.Void(request.PlayerId,
                               request.OperatorId,
                               request.SiteId,
                               request.GameRoundId,
                               request.GameTypeId,
                               request.FreeBet).Returns(true);

            //Act
            var result = await voidGameService.VoidBankingGameTicket<BlackjackRoundEntity, BlackjackBetEntity>(request);

            //Assert
            Assert.True(result.IsSuccessful());
            Assert.Equal(0, result.ErrorCode);
        }

        [Theory, AutoData]
        public async Task VoidBlackJackTicket_RunningTicket_VoidHandFailed_ReturnCorrectResult(VoidTicketRequest request)
        {
            //Arrange
            var blackJackEntity = new BlackjackBetEntity
            {
                SiteId = "4100300",
                HandId = 1,
                Round = 34
            };
            var roundIds = new List<long> { 1, 2 };
            var blackjackRecord = new BlackjackRecord(blackJackEntity, false, roundIds);
            ticketHandler.ExistBankingGameTicket<BlackjackBetEntity>(request.PlayerId,
                    request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.FreeBet).Returns(blackjackRecord);
            var apiResult = new VoidGameApiResult
            {
                Result = new VoidGameResult
                {
                    ErrorCode = 1000,
                    ErrorMessage = "Tickets was moved unsuccessfully."
                }
            };
            voidGameApi.VoidGameTicket(request).Returns(apiResult);
            ticketHandler.Void(request.PlayerId,
            request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                      request.GameTypeId,
                    request.FreeBet).Returns(false);
            //Act
            var result = await voidGameService.VoidBankingGameTicket<BlackjackRoundEntity, BlackjackBetEntity>(request);

            //Assert
            Assert.True(result.IsFailed());
            Assert.Equal(1000, result.ErrorCode);
            Assert.Equal("Tickets was moved unsuccessfully.", result.ErrorMessage);
        }

        [Theory, AutoData]
        public async Task VoidBlackJackTicket_RunningTicket_MoveRoundFailed_ReturnCorrectResult(VoidTicketRequest request)
        {
            //Arrange
            var blackJackEntity = new BlackjackBetEntity
            {
                SiteId = "4100300",
                HandId = 1,
                Round = 34
            };
            var roundIds = new List<long> { 1, 2 };
            var blackjackRecord = new BlackjackRecord(blackJackEntity, false, roundIds);
            ticketHandler.ExistBankingGameTicket<BlackjackBetEntity>(request.PlayerId,
                    request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.FreeBet).Returns(blackjackRecord);
            var apiResult = new VoidGameApiResult
            {
                Result = new VoidGameResult
                {
                    ErrorCode = 0,
                }
            };
            voidGameApi.VoidGameTicket(request).Returns(apiResult);
            ticketHandler.Void(request.PlayerId,
            request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.GameTypeId,
                    request.FreeBet).Returns(true);
            ticketHandler.MoveBankingGameRound<BlackjackRoundEntity>(blackJackEntity.Round).Returns(false);
            //Act
            var result = await voidGameService.VoidBankingGameTicket<BlackjackRoundEntity, BlackjackBetEntity>(request);

            //Assert
            Assert.True(result.IsFailed());
            Assert.Equal(1000, result.ErrorCode);
            Assert.Equal("Tickets was moved unsuccessfully.", result.ErrorMessage);
        }

        [Fact]
        public async Task VoidBlackJackTicket_RunningTicket_CallServiceSuccessfully_ReturnCorrectResult()
        {
            //Arrange
            var request = new VoidTicketRequest(111, "1W", "234234", 34, 23, "esther test", false);
            var blackJackEntity = new BlackjackBetEntity
            {
                SiteId = "4100300",
                HandId = 1,
                Round = 34,
                Uid = "111"
            };
            var roundIds = new List<long> { 34 };
            var blackjackRecord = new BlackjackRecord(blackJackEntity, false, roundIds);
            ticketHandler.ExistBankingGameTicket<BlackjackBetEntity>(request.PlayerId,
                    request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.FreeBet).Returns(blackjackRecord);
            var apiResult = new VoidGameApiResult
            {
                Result = new VoidGameResult
                {
                    ErrorCode = 0,
                }
            };
            voidGameApi.VoidGameTicket(request).Returns(apiResult);
            ticketHandler.Void(request.PlayerId,
            request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.GameTypeId,
                    request.FreeBet).Returns(true);
            ticketHandler.MoveBankingGameRound<BlackjackRoundEntity>(blackJackEntity.Round).Returns(true);
            voidRunningTicketApiFactory.Setup(factory => factory.GetVoidRunningTicketApi(It.IsAny<string>()))
                .Returns(Mock.Of<IVoidRunningTicketApi>());
            var voidRunningTicketApi = new Mock<IVoidRunningTicketApi>();
            voidRunningTicketApi.Setup(api => api.VoidTicket(It.IsAny<VoidRunningTicketRequest>()))
                .ReturnsAsync(true);
            voidRunningTicketApiFactory.Setup(factory => factory.GetVoidRunningTicketApi(It.IsAny<string>()))
                .Returns(voidRunningTicketApi.Object);
            //Act
            var result = await voidGameService.VoidBankingGameTicket<BlackjackRoundEntity, BlackjackBetEntity>(request);

            //Assert
            Assert.True(result.IsSuccessful());
        }

        [Fact]
        public async Task VoidBlackJackTicket_RunningTicket_CallServiceUnSuccessfully_ReturnErrorCode()
        {
            //Arrange
            var request = new VoidTicketRequest(111, "1W", "234234", 34, 23, "esther test", false);
            var blackJackEntity = new BlackjackBetEntity
            {
                SiteId = "4100300",
                HandId = 1,
                Round = 34,
            };
            var roundIds = new List<long> { 34 };
            var blackjackRecord = new BlackjackRecord(blackJackEntity, false, roundIds);
            ticketHandler.ExistBankingGameTicket<BlackjackBetEntity>(request.PlayerId,
                    request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.FreeBet).Returns(blackjackRecord);
            var apiResult = new VoidGameApiResult
            {
                Result = new VoidGameResult
                {
                    ErrorCode = 0,
                }
            };
            voidGameApi.VoidGameTicket(request).Returns(apiResult);
            ticketHandler.Void(request.PlayerId,
            request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.GameTypeId,
                    request.FreeBet).Returns(true);
            ticketHandler.MoveBankingGameRound<BlackjackRoundEntity>(blackJackEntity.Round).Returns(true);
            accountInfoRepository.Setup(repo => repo.FindOneAsync(It.IsAny<Expression<Func<AccountInfoEntity, bool>>>()))
                .ReturnsAsync(new AccountInfoEntity { Name = "a" });
            voidRunningTicketApiFactory.Setup(factory => factory.GetVoidRunningTicketApi(It.IsAny<string>()))
                .Returns(Mock.Of<IVoidRunningTicketApi>());
            var voidRunningTicketApi = new Mock<IVoidRunningTicketApi>();
            voidRunningTicketApi.Setup(api => api.VoidTicket(It.IsAny<VoidRunningTicketRequest>()))
                .ReturnsAsync(false);
            voidRunningTicketApiFactory.Setup(factory => factory.GetVoidRunningTicketApi(It.IsAny<string>()))
                .Returns(voidRunningTicketApi.Object);
            //Act
            var result = await voidGameService.VoidBankingGameTicket<BlackjackRoundEntity, BlackjackBetEntity>(request);

            //Assert
            Assert.True(result.IsFailed());
            Assert.Equal("Request void running ticket unsuccessfully.", result.ErrorMessage);
        }

        [Fact]
        public async Task VoiHoldemPokerTicket_RunningTicket_Successfully_ReturnCorrectResult()
        {
            //Arrange
            var request = new VoidTicketRequest(111, "1W", "234234", 34, 39, "esther test", false);
            var pokerEntity = new List<HoldemPokerBetEntity>
            {
                new HoldemPokerBetEntity
                 {
                     SiteId = "4100300",
                     Round = 34,
                     Paid = false,
                     Uid = "111"
                 }
            };
            var pokerRecord = new GameTicketRecord<HoldemPokerBetEntity>(pokerEntity, false);
            ticketHandler.ExistHoldemPoker(request.PlayerId,
                    request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.GameTypeId,
                    request.FreeBet).Returns(pokerRecord);
            var apiResult = new VoidGameApiResult
            {
                Result = new VoidGameResult
                {
                    ErrorCode = 0,
                }
            };

            voidGameApi.VoidGameTicket(request).Returns(apiResult);
            ticketHandler.Void(request.PlayerId,
            request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.GameTypeId,
                    request.FreeBet).Returns(true);
            accountInfoRepository.Setup(repo => repo.FindOneAsync(It.IsAny<Expression<Func<AccountInfoEntity, bool>>>()))
                .ReturnsAsync(new AccountInfoEntity { Name = "a" });
            voidRunningTicketApiFactory.Setup(factory => factory.GetVoidRunningTicketApi(It.IsAny<string>()))
                .Returns(Mock.Of<IVoidRunningTicketApi>());
            var voidRunningTicketApi = new Mock<IVoidRunningTicketApi>();
            voidRunningTicketApi.Setup(api => api.VoidTicket(It.IsAny<VoidRunningTicketRequest>()))
                .ReturnsAsync(true);
            voidRunningTicketApiFactory.Setup(factory => factory.GetVoidRunningTicketApi(It.IsAny<string>()))
                .Returns(voidRunningTicketApi.Object);
            //Act
            var result = await voidGameService.VoidHoldemPokerTicket(request);

            //Assert
            Assert.True(result.IsSuccessful());
        }
    }
}
