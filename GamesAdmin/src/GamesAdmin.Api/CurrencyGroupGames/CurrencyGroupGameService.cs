using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared.Helpers;
using GamesAdmin.Api.CurrencyGroupGames.Requests;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Currencies;
using GamesAdmin.Database.Entities.Chips;
using Microsoft.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentry;
using Microsoft.CodeAnalysis.CSharp;
using System.Globalization;

namespace GamesAdmin.Api.CurrencyGroupGames
{
    public interface ICurrencyGroupGameService
    {
        Task<IEnumerable<CurrencyGroupGame>> GetAll();

        Task<CurrencyGroupGame> Get(int gameId, string currencyGroupId);

        Task<bool> Update(CurrencyGroupGame currencyGroupGame);

        Task<IEnumerable<CurrencyGroupGame>> GetByGroup(string currencyGroupId);

        Task<IEnumerable<CurrencyGroupGame>> GetByGameId(int gameId);

        Task<bool> AddRange(IEnumerable<CurrencyGroupGame> currencyGroupGames);

        Task<CurrencyGroupSuggestionsResult> GetSuggestions(CurrencyGroupProposal proposal);

        Task<IEnumerable<CurrencyGroupGame>> SuggestNewRange(CurrencyGroupProposal proposal);
    }

    public class CurrencyGroupGameService : ICurrencyGroupGameService
    {
        private readonly IGenericRepository<CurrencyGroupGameEntity> currencyGroupGameRepository;
        private readonly IMapper mapper;
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<CurrencyGroupEntity> currencyGroupRepository;
        private readonly IGenericRepository<GameConfigEntity> gameConfigRepository;
        private readonly IGenericRepository<ChipEntity> chipRepository;

        public CurrencyGroupGameService(
            IGenericRepository<CurrencyGroupGameEntity> currencyGroupGameRepository,
            IMapper mapper,
            ISentryClient sentryClient,
            IGenericRepository<CurrencyGroupEntity> currencyGroupRepository,
            IGenericRepository<GameConfigEntity> gameConfigRepository,
            IGenericRepository<ChipEntity> chipRepository)
        {
            this.currencyGroupGameRepository = currencyGroupGameRepository;
            this.mapper = mapper;
            this.sentryClient = sentryClient;
            this.currencyGroupRepository = currencyGroupRepository;
            this.gameConfigRepository = gameConfigRepository;
            this.chipRepository = chipRepository;
        }

        public async Task<CurrencyGroupGame> Get(int gameId, string currencyGroupId)
        {
            try
            {
                var gameConfig = await currencyGroupGameRepository.FindOneAsync(entity => entity.GameId == gameId && entity.GroupId == ObjectId.Parse(currencyGroupId));

                return mapper.Map<CurrencyGroupGame>(gameConfig);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }

        public async Task<IEnumerable<CurrencyGroupGame>> GetByGroup(string currencyGroupId)
        {
            try
            {
                var gameConfigs = await currencyGroupGameRepository.FilterByAsync(entity => entity.GroupId == ObjectId.Parse(currencyGroupId));

                return gameConfigs.Select(gameConfig => mapper.Map<CurrencyGroupGame>(gameConfig));
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }


        public async Task<IEnumerable<CurrencyGroupGame>> GetByGameId(int gameId)
        {
            try
            {
                var gameConfigs = await currencyGroupGameRepository.FilterByAsync(entity => entity.GameId == gameId);

                return gameConfigs.Select(gameConfig => mapper.Map<CurrencyGroupGame>(gameConfig));
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }


        public Task<IEnumerable<CurrencyGroupGame>> GetAll()
        => Task.Run(() =>
        {
            try
            {
                var currencyGroupGames = currencyGroupGameRepository.AsQueryable().ToList();

                return currencyGroupGames.Select(entity => mapper.Map<CurrencyGroupGame>(entity));
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                
                return Enumerable.Empty<CurrencyGroupGame>();
            }
        });

        public async Task<bool> Update(CurrencyGroupGame currencyGroupGame)
        {
            try
            {
                var gameIds = new List<int>(currencyGroupGame.OtherGameIdsInGroup) { currencyGroupGame.GameId };
                var currencyGroupGames = (await currencyGroupGameRepository.FilterByAsync(
                    entity => gameIds.Contains(entity.GameId) && entity.GroupId == ObjectId.Parse(currencyGroupGame.GroupId))).ToList();
                var gameConfigs = (await gameConfigRepository.FilterByAsync(x => gameIds.Contains(x.GameId))).ToList();

                var writes = new List<WriteModel<CurrencyGroupGameEntity>>();

                foreach (var gameId in gameIds)
                {
                    var groupGameConfig = currencyGroupGames.FirstOrDefault(x => x.GameId == gameId);

                    if (groupGameConfig != null)
                    {
                        var group = await currencyGroupRepository.FindByIdAsync(groupGameConfig.GroupId.ToString());
                        var changedSettings = SettingHelper.GetCurrencyGroupGameChangedModel(groupGameConfig, currencyGroupGame);
                        sentryClient.CaptureChangedSettings(groupGameConfig.GameName, "Partner's game setting", $" ({group.PartnerId} - {group.Name})", changedSettings);
                    }

                    var gameConfig = gameConfigs.FirstOrDefault(x => x.GameId == gameId);
                    var sortedCoinValues = currencyGroupGame.CoinValues.OrderBy(x => x).ToArray();
                    var sortedBetLevels = currencyGroupGame.BetLevels.OrderBy(x => x).ToArray();

                    var update = Builders<CurrencyGroupGameEntity>.Update
                        .Set(field => field.Enabled, currencyGroupGame.Enabled)
                        .Set(field => field.MinBet, currencyGroupGame.MinBet)
                        .Set(field => field.MaxBet, currencyGroupGame.MaxBet)
                        .Set(field => field.MaxBetPerChoices, currencyGroupGame.MaxBetPerChoices)
                        .Set(field => field.Chips, currencyGroupGame.Chips)
                        .Set(field => field.CoinValues, sortedCoinValues)
                        .Set(field => field.DefaultCoinValue, currencyGroupGame.DefaultCoinValue ?? sortedCoinValues.FirstOrDefault())
                        .Set(field => field.DefaultBetLevel, currencyGroupGame.DefaultBetLevel ?? sortedBetLevels.FirstOrDefault())
                        .Set(field => field.BetLevels, currencyGroupGame.BetLevels.OrderBy(x => x).ToArray() ?? new double[10])
                        .Set(field => field.MaxWinPerRound, currencyGroupGame.MaxWinPerRound)
                        .Set(field => field.GameName, gameConfig?.Name ?? currencyGroupGame.GameName);

                    var filterBuilder = Builders<CurrencyGroupGameEntity>.Filter;
                    var filterDefinition = filterBuilder.And(
                        filterBuilder.Eq(x => x.GroupId, ObjectId.Parse(currencyGroupGame.GroupId)),
                        filterBuilder.Eq(x => x.GameId, gameId));

                    writes.Add(new UpdateOneModel<CurrencyGroupGameEntity>(filterDefinition, update) { IsUpsert = true });
                }

                await currencyGroupGameRepository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });
                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<bool> AddRange(IEnumerable<CurrencyGroupGame> currencyGroupGames)
        {
            try
            {
                var writes = new List<WriteModel<CurrencyGroupGameEntity>>();
                var gameIds = currencyGroupGames.Select(x => x.GameId).ToList();
                var gameConfigs = (await gameConfigRepository.FilterByAsync(x => gameIds.Contains(x.GameId))).ToList();

                foreach (var currencyGroupGame in currencyGroupGames)
                {
                    var gameConfig = gameConfigs.FirstOrDefault(x => x.GameId == currencyGroupGame.GameId);
                    var sortedCoinValues = currencyGroupGame.CoinValues.OrderBy(x => x).ToArray();
                    var sortedBetLevels = currencyGroupGame.BetLevels.OrderBy(x => x).ToArray();

                    var update = Builders<CurrencyGroupGameEntity>.Update
                        .Set(field => field.Enabled, currencyGroupGame.Enabled)
                        .Set(field => field.MinBet, currencyGroupGame.MinBet)
                        .Set(field => field.MaxBet, currencyGroupGame.MaxBet)
                        .Set(field => field.MaxBetPerChoices, currencyGroupGame.MaxBetPerChoices)
                        .Set(field => field.Chips, currencyGroupGame.Chips)
                        .Set(field => field.CoinValues, sortedCoinValues)
                        .Set(field => field.DefaultCoinValue, currencyGroupGame.DefaultCoinValue ?? sortedCoinValues.FirstOrDefault())
                        .Set(field => field.DefaultBetLevel, currencyGroupGame.DefaultBetLevel ?? sortedBetLevels.FirstOrDefault())
                        .Set(field => field.BetLevels, currencyGroupGame.BetLevels.OrderBy(x => x).ToArray() ?? new double[10])
                        .Set(field => field.MaxWinPerRound, currencyGroupGame.MaxWinPerRound)
                        .Set(field => field.GameName, gameConfig?.Name ?? currencyGroupGame.GameName);

                    var filterBuilder = Builders<CurrencyGroupGameEntity>.Filter;
                    var filterDefinition = filterBuilder.And(
                        filterBuilder.Eq(x => x.GroupId, ObjectId.Parse(currencyGroupGame.GroupId)),
                        filterBuilder.Eq(x => x.GameId, currencyGroupGame.GameId));

                    writes.Add(new UpdateOneModel<CurrencyGroupGameEntity>(filterDefinition, update) { IsUpsert = true });
                }

                await currencyGroupGameRepository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });
                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<CurrencyGroupSuggestionsResult> GetSuggestions(CurrencyGroupProposal proposal)
        {
            try
            {
                var baseCurrencyGroupGames = await GetBaseCurrencyGroupGames(proposal);
                var candidateGroups = await GetCandidateCurrencyGroups(proposal);
                
                var suggestions = await ProcessCandidateGroups(candidateGroups, baseCurrencyGroupGames, proposal);
                
                return CreateSuggestionsResult(suggestions);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return new CurrencyGroupSuggestionsResult();
            }
        }

        private async Task<Dictionary<string, CurrencyGroupGameEntity>> GetBaseCurrencyGroupGames(CurrencyGroupProposal proposal)
        {
            Expression<Func<CurrencyGroupGameEntity, bool>> filter;
            
            if (proposal.GetFromAllGames || proposal.GameNames?.Any() == true)
            {
                filter = x => x.GroupId == ObjectId.Parse(proposal.BaseCurrencyGroupId) && 
                              (proposal.GetFromAllGames || proposal.GameNames.Contains(x.GameName));
            }
            else
            {
                filter = x => x.GroupId == ObjectId.Parse(proposal.BaseCurrencyGroupId);
            }

            var baseGames = await currencyGroupGameRepository.FilterByAsync(filter);
            return baseGames.ToDictionary(x => x.GameName);
        }

        private async Task<ILookup<ObjectId, CurrencyGroupGameEntity>> GetCandidateCurrencyGroups(CurrencyGroupProposal proposal)
        {
            Expression<Func<CurrencyGroupGameEntity, bool>> filter;
            
            if (proposal.GetFromAllGames || proposal.GameNames?.Any() == true)
            {
                filter = x => proposal.GetFromAllGames || proposal.GameNames.Contains(x.GameName);
            }
            else
            {
                filter = x => true;
            }

            var candidateGames = await currencyGroupGameRepository.FilterByAsync(filter);
            return candidateGames.ToLookup(x => x.GroupId);
        }

        private async Task<List<CurrencyGroupSuggestion>> ProcessCandidateGroups(
            ILookup<ObjectId, CurrencyGroupGameEntity> candidateGroups,
            Dictionary<string, CurrencyGroupGameEntity> baseCurrencyGroupGames,
            CurrencyGroupProposal proposal)
        {
            var suggestions = new List<CurrencyGroupSuggestion>();
            var baseGameNames = baseCurrencyGroupGames.Keys.ToHashSet();

            foreach (var group in candidateGroups)
            {
                var groupGames = group.ToList();
                var candidateGameNames = groupGames.Select(g => g.GameName).ToHashSet();
                if (!baseGameNames.IsSubsetOf(candidateGameNames))
                {
                    continue;
                }    
                var groupId = group.Key.ToString();
                var groupEntity = await currencyGroupRepository.FindByIdAsync(groupId);
                
                if (groupEntity == null)
                {
                    continue;
                }    

                var suggestion = CreateGroupSuggestion(groupGames, groupEntity, baseCurrencyGroupGames, proposal);
                if (suggestion != null)
                {
                    suggestions.Add(suggestion);
                }
            }

            return suggestions;
        }

        private CurrencyGroupSuggestion CreateGroupSuggestion(
            List<CurrencyGroupGameEntity> groupGames,
            CurrencyGroupEntity groupEntity,
            Dictionary<string, CurrencyGroupGameEntity> baseCurrencyGroupGames,
            CurrencyGroupProposal proposal)
        {
            var groupSuggestion = new CurrencyGroupSuggestion
            {
                GroupId = groupEntity.Id.ToString(),
                GroupName = groupEntity.Name,
                PartnerId = groupEntity.PartnerId,
                Rate = proposal.Rate,
                GameComparisons = new List<GameBetLimitComparison>()
            };

            var (Comparisons, TotalValueDifference, HasSmallerThanExpected) = ProcessGameComparisons(groupGames, baseCurrencyGroupGames, proposal);
            
            groupSuggestion.GameComparisons = Comparisons;
            groupSuggestion.ValueDifference = TotalValueDifference;
            groupSuggestion.IsSmallerThanExpected = HasSmallerThanExpected;

            return groupSuggestion;
        }

        private (List<GameBetLimitComparison> Comparisons, double TotalValueDifference, bool HasSmallerThanExpected) 
            ProcessGameComparisons(
                List<CurrencyGroupGameEntity> groupGames,
                Dictionary<string, CurrencyGroupGameEntity> baseCurrencyGroupGames,
                CurrencyGroupProposal proposal)
        {
            var comparisons = new List<GameBetLimitComparison>();
            double totalValueDifference = 0;
            bool hasSmallerThanExpected = false;

            foreach (var game in groupGames)
            {
                if (!baseCurrencyGroupGames.TryGetValue(game.GameName, out var baseGame))
                {
                    continue;
                }    

                var gameComparison = CreateGameBetLimitComparison(baseGame, game, proposal.Rate);
                comparisons.Add(gameComparison);

                totalValueDifference += CalculateTotalDifference(gameComparison);
                hasSmallerThanExpected |= HasAnySmallerThanExpected(gameComparison);
            }

            return (comparisons, totalValueDifference, hasSmallerThanExpected);
        }

        private GameBetLimitComparison CreateGameBetLimitComparison(
            CurrencyGroupGameEntity baseGame,
            CurrencyGroupGameEntity candidateGame,
            double rate)
        {
            return new GameBetLimitComparison
            {
                GameId = candidateGame.GameId,
                GameName = candidateGame.GameName,
                MinBetComparison = CreateBetLimitComparison(baseGame.MinBet, candidateGame.MinBet, rate),
                MaxBetComparison = CreateBetLimitComparison(baseGame.MaxBet, candidateGame.MaxBet, rate),
                MaxBetPerChoiceComparison = CreateMaxBetPerChoiceComparison(baseGame, candidateGame, rate),
                MinCoinValueComparison = CreateMinArrayBetLimitComparison(baseGame.CoinValues, candidateGame.CoinValues, rate),
                MaxCoinValueComparison = CreateMaxArrayBetLimitComparison(baseGame.CoinValues, candidateGame.CoinValues, rate),
                MinBetLevelComparison = CreateMinArrayBetLimitComparison(baseGame.BetLevels, candidateGame.BetLevels, rate),
                MaxBetLevelComparison = CreateMaxArrayBetLimitComparison(baseGame.BetLevels, candidateGame.BetLevels, rate)
            };
        }

        private BetLimitComparison CreateMaxBetPerChoiceComparison(
            CurrencyGroupGameEntity baseGame,
            CurrencyGroupGameEntity candidateGame,
            double rate)
        {
            var baseMaxBetPerChoice = GetMaxBetPerChoiceValue(baseGame.MaxBetPerChoices);
            var candidateMaxBetPerChoice = GetMaxBetPerChoiceValue(candidateGame.MaxBetPerChoices);
            
            return CreateBetLimitComparison(baseMaxBetPerChoice, candidateMaxBetPerChoice, rate);
        }

        private static double GetMaxBetPerChoiceValue(Dictionary<string, double> maxBetPerChoices)
        {
            return maxBetPerChoices?.Values?.Any() == true ? maxBetPerChoices.Values.Max() : 0;
        }

        private static double CalculateTotalDifference(GameBetLimitComparison comparison)
        {
            return Math.Abs(comparison.MinBetComparison.Difference) +
                   Math.Abs(comparison.MaxBetComparison.Difference) +
                   Math.Abs(comparison.MaxBetPerChoiceComparison.Difference) +
                   Math.Abs(comparison.MinCoinValueComparison.Difference) +
                   Math.Abs(comparison.MaxCoinValueComparison.Difference) +
                   Math.Abs(comparison.MinBetLevelComparison.Difference) +
                   Math.Abs(comparison.MaxBetLevelComparison.Difference);
        }

        private static bool HasAnySmallerThanExpected(GameBetLimitComparison comparison)
        {
            var comparisons = new[]
            {
                comparison.MaxBetComparison.IsSmallerThanExpected,
                comparison.MaxBetPerChoiceComparison.IsSmallerThanExpected,
                comparison.MaxCoinValueComparison.IsSmallerThanExpected,
                comparison.MaxBetLevelComparison.IsSmallerThanExpected
            };

            return comparisons.Any(isSmaller => isSmaller);
        }

        private CurrencyGroupSuggestionsResult CreateSuggestionsResult(List<CurrencyGroupSuggestion> suggestions)
        {
            if (!suggestions.Any())
            {
                return new CurrencyGroupSuggestionsResult();
            }

            var bestSuggestion = suggestions.OrderBy(s => s.ValueDifference).First();
            var bestSmallerSuggestion = suggestions
                .Where(s => s.IsSmallerThanExpected)
                .OrderBy(s => s.ValueDifference)
                .FirstOrDefault();

            return new CurrencyGroupSuggestionsResult
            {
                BestSuggestion = bestSuggestion,
                BestSmallerSuggestion = bestSmallerSuggestion
            };
        }

        private BetLimitComparison CreateBetLimitComparison(double baseValue, double candidateValue, double rate)
        {
            var expectedValue = baseValue / rate;
            var difference = candidateValue - expectedValue;
            var differencePercentage = Math.Abs(expectedValue) > double.Epsilon ? difference / expectedValue * 100 : 0;
            
            return new BetLimitComparison
            {
                BaseValue = baseValue,
                CandidateValue = candidateValue,
                ExpectedValue = expectedValue,
                Difference = difference,
                DifferencePercentage = differencePercentage,
                IsSmallerThanExpected = candidateValue < expectedValue
            };
        }

        private BetLimitComparison CreateMinArrayBetLimitComparison(double[] baseValues, double[] candidateValues, double rate)
        {
            if (baseValues == null || candidateValues == null || baseValues.Length == 0 || candidateValues.Length == 0)
            {
                return new BetLimitComparison();
            }    
                
            var baseMin = baseValues.Min();
            var candidateMin = candidateValues.Min();
            return CreateBetLimitComparison(baseMin, candidateMin, rate);
        }

        private BetLimitComparison CreateMaxArrayBetLimitComparison(double[] baseValues, double[] candidateValues, double rate)
        {
            if (baseValues == null || candidateValues == null || baseValues.Length == 0 || candidateValues.Length == 0)
            {
                return new BetLimitComparison();
            }    
                
            var baseMax = baseValues.Max();
            var candidateMax = candidateValues.Max();
            return CreateBetLimitComparison(baseMax, candidateMax, rate);
        }

        public async Task<IEnumerable<CurrencyGroupGame>> SuggestNewRange(CurrencyGroupProposal proposal)
        {
            try
            {
                var baseCurrencyGroupGames = await GetBaseCurrencyGroupGames(proposal);
                var result = new List<CurrencyGroupGame>();
                var availableChipValues = GetAvailableChipValues();

                foreach (var baseGame in baseCurrencyGroupGames.Values)
                {
                    var adjustedGame = CreateAdjustedCurrencyGroupGame(baseGame, proposal.Rate, availableChipValues);
                    result.Add(adjustedGame);
                }

                return result;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return Enumerable.Empty<CurrencyGroupGame>();
            }
        }

        private static CurrencyGroupGame CreateAdjustedCurrencyGroupGame(CurrencyGroupGameEntity baseGame, double rate, List<int> availableChipValues)
        {
            var adjustedCoinValues = AdjustArrayValues(baseGame.CoinValues, rate);
            var adjustedBetLevels = AdjustArrayValues(baseGame.BetLevels, rate);
            var adjustedChips = AdjustChips(baseGame.Chips, rate, availableChipValues);
            
            return new CurrencyGroupGame(
                groupId: "",
                gameId: baseGame.GameId,
                gameName: baseGame.GameName,
                enabled: false,
                minbet: baseGame.MinBet / rate,
                maxbet: baseGame.MaxBet / rate,
                maxBetPerChoices: AdjustMaxBetPerChoices(baseGame.MaxBetPerChoices, rate),
                chips: adjustedChips,
                betLevels: adjustedBetLevels,
                stakeLevels: baseGame.StakeLevels ?? new StakeLevel[0],
                coinValues: adjustedCoinValues,
                maxWinPerRound: baseGame.MaxWinPerRound / rate,
                defaultCoinValue: adjustedCoinValues?.Length > 0 ? adjustedCoinValues.Min() : (double?)null,
                defaultBetLevel: adjustedBetLevels?.Length > 0 ? adjustedBetLevels.Min() : (double?)null
            );
        }

        private static string[] AdjustChips(string[] chips, double rate, List<int> availableChipValues)
        {
            if (chips == null)
            {
                return new string[0];
            }

            if (!availableChipValues.Any())
            {
                return chips;
            }

            return chips.Select(chipValue => 
            {
                if (int.TryParse(chipValue, out var value))
                {
                    var adjustedValue = RoundToNearestChipValue(value / rate, availableChipValues);
                    return adjustedValue.ToString(CultureInfo.CurrentCulture);
                }
                return chipValue;
            }).ToArray();
        }

        private List<int> GetAvailableChipValues()
        {
            try
            {
                var chips = chipRepository.AsQueryable().ToList();
                return chips.Select(c => c.Value).OrderBy(v => v).ToList();
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return new List<int>();
            }
        }

        private static double RoundToNearestChipValue(double value, List<int> availableChipValues)
        {
            if (!availableChipValues.Any())
            {
                return value;
            }

            var targetValue = (int)Math.Round(value);
            var nearestValue = availableChipValues
                .OrderBy(cv => Math.Abs(cv - targetValue))
                .First();

            return nearestValue;
        }

        private static Dictionary<string, double> AdjustMaxBetPerChoices(Dictionary<string, double> maxBetPerChoices, double rate)
        {
            if (maxBetPerChoices == null)
            {
                return new Dictionary<string, double>();
            }

            return maxBetPerChoices.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value / rate
            );
        }

        private static double[] AdjustArrayValues(double[] values, double rate)
        {
            if (values == null)
            {
                return new double[0];
            }

            return values.Select(value => value / rate).ToArray();
        }
    }
}