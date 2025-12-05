using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetLimitSuggestion;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Chips;
using GamesAdmin.Database.Entities.Currencies;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Sentry;

namespace GamesAdmin.Api.BetLimitSuggestion
{
    public interface IBetLimitSuggestionService
    {
        Task<Dictionary<string, CurrencyGroupGameEntity>> GetBaseCurrencyGroupGames(CurrencyGroupProposal proposal, bool filterByBaseGameNames = true);
        Task<ILookup<ObjectId, CurrencyGroupGameEntity>> GetCandidateCurrencyGroups(CurrencyGroupProposal proposal);
        Task<List<CurrencyGroupSuggestion>> ProcessCandidateGroups(ILookup<ObjectId,
            CurrencyGroupGameEntity> candidateGroups,
            Dictionary<string, CurrencyGroupGameEntity> baseCurrencyGroupGames,
            CurrencyGroupProposal proposal);
        CurrencyGroupSuggestion CreateGroupSuggestion(List<CurrencyGroupGameEntity> groupGames,
            CurrencyGroupEntity groupEntity, Dictionary<string,
            CurrencyGroupGameEntity> baseCurrencyGroupGames,
            CurrencyGroupProposal proposal);
        Task<BetLimitSuggestionResult> CreateSuggestionsResultAsync(List<CurrencyGroupSuggestion> suggestions);
        CurrencyGroupGame CreateAdjustedCurrencyGroupGame(CurrencyGroupGameEntity baseGame, double rate, List<int> availableChipValues);
        List<int> GetAvailableChipValues();
    }
    public class BetLimitSuggestionService : IBetLimitSuggestionService
    {
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<CurrencyGroupGameEntity> currencyGroupGameRepository;
        private readonly IGenericRepository<CurrencyGroupEntity> currencyGroupRepository;
        private readonly IGenericRepository<ChipEntity> chipRepository;
        private readonly IGenericRepository<GameConfigEntity> gameConfigRepository;
        private readonly List<string> baseGameNames;

        public BetLimitSuggestionService(
          ISentryClient sentryClient,
          IGenericRepository<CurrencyGroupGameEntity> currencyGroupGameRepository,
          IGenericRepository<CurrencyGroupEntity> currencyGroupRepository,
          IGenericRepository<ChipEntity> chipRepository,
          IGenericRepository<GameConfigEntity> gameConfigRepository,
          IConfiguration configuration)
        {
            this.sentryClient = sentryClient;
            this.currencyGroupGameRepository = currencyGroupGameRepository;
            this.currencyGroupRepository = currencyGroupRepository;
            this.chipRepository = chipRepository;
            this.gameConfigRepository = gameConfigRepository;
            baseGameNames = configuration.GetSection("BaseGameForSuggestion").Get<List<string>>();
        }

        public async Task<Dictionary<string, CurrencyGroupGameEntity>> GetBaseCurrencyGroupGames(CurrencyGroupProposal proposal, bool filterByBaseGameNames = true)
        {
            Expression<Func<CurrencyGroupGameEntity, bool>> filter;

            filter = x => x.GroupId == ObjectId.Parse(proposal.BaseCurrencyGroupId) &&
                            (!filterByBaseGameNames || baseGameNames.Contains(x.GameName));

            var baseGames = await currencyGroupGameRepository.FilterByAsync(filter);
            return baseGames.ToDictionary(x => x.GameName);
        }

        public async Task<ILookup<ObjectId, CurrencyGroupGameEntity>> GetCandidateCurrencyGroups(CurrencyGroupProposal proposal)
        {
            Expression<Func<CurrencyGroupGameEntity, bool>> filter;

            filter = x => proposal.CandidateCurrencyGroupIds.Contains(x.GroupId.ToString()) &&
                            baseGameNames.Contains(x.GameName);

            var candidateGames = await currencyGroupGameRepository.FilterByAsync(filter);
            return candidateGames.ToLookup(x => x.GroupId);
        }

        public async Task<List<CurrencyGroupSuggestion>> ProcessCandidateGroups(
            ILookup<ObjectId, CurrencyGroupGameEntity> candidateGroups,
            Dictionary<string, CurrencyGroupGameEntity> baseCurrencyGroupGames,
            CurrencyGroupProposal proposal)
        {
            var suggestions = new List<CurrencyGroupSuggestion>();

            foreach (var group in candidateGroups)
            {
                var groupGames = group.ToList();
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

        public CurrencyGroupSuggestion CreateGroupSuggestion(
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
                Rate = proposal.RateToBase,
                GameComparisons = new List<GameBetLimitComparison>()
            };

            var (Comparisons, TotalValueDifference, HasSmallerThanExpected) = ProcessGameComparisons(groupGames, baseCurrencyGroupGames, proposal);

            groupSuggestion.GameComparisons = Comparisons;
            groupSuggestion.ValueDifference = TotalValueDifference;
            groupSuggestion.IsSmallerThanExpected = HasSmallerThanExpected;

            return groupSuggestion;
        }

        public (List<GameBetLimitComparison> Comparisons, double TotalValueDifference, bool HasSmallerThanExpected)
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

                var gameComparison = CreateGameBetLimitComparison(baseGame, game, proposal.RateToBase);
                comparisons.Add(gameComparison);

                totalValueDifference += CalculateTotalDifference(gameComparison);
                hasSmallerThanExpected |= HasAnySmallerThanExpected(gameComparison);
            }

            return (comparisons, totalValueDifference, hasSmallerThanExpected);
        }

        public GameBetLimitComparison CreateGameBetLimitComparison(
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

        public BetLimitComparison CreateMaxBetPerChoiceComparison(
            CurrencyGroupGameEntity baseGame,
            CurrencyGroupGameEntity candidateGame,
            double rate)
        {
            var baseMaxBetPerChoice = GetMaxBetPerChoiceValue(baseGame.MaxBetPerChoices);
            var candidateMaxBetPerChoice = GetMaxBetPerChoiceValue(candidateGame.MaxBetPerChoices);

            return CreateBetLimitComparison(baseMaxBetPerChoice, candidateMaxBetPerChoice, rate);
        }

        public static double GetMaxBetPerChoiceValue(Dictionary<string, double> maxBetPerChoices)
        {
            return maxBetPerChoices?.Values?.Any() == true ? maxBetPerChoices.Values.Max() : 0;
        }

        public static double CalculateTotalDifference(GameBetLimitComparison comparison)
        {
            return Math.Abs(comparison.MinBetComparison.Difference) +
                   Math.Abs(comparison.MaxBetComparison.Difference) +
                   Math.Abs(comparison.MaxBetPerChoiceComparison.Difference) +
                   Math.Abs(comparison.MinCoinValueComparison.Difference) +
                   Math.Abs(comparison.MaxCoinValueComparison.Difference) +
                   Math.Abs(comparison.MinBetLevelComparison.Difference) +
                   Math.Abs(comparison.MaxBetLevelComparison.Difference);
        }

        public static bool HasAnySmallerThanExpected(GameBetLimitComparison comparison)
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

        public async Task<BetLimitSuggestionResult> CreateSuggestionsResultAsync(List<CurrencyGroupSuggestion> suggestions)
        {
            if (!suggestions.Any())
            {
                return new BetLimitSuggestionResult();
            }

            var bestSuggestion = suggestions.OrderBy(s => s.ValueDifference).First();
            var bestSmallerSuggestion = suggestions
                .Where(s => s.IsSmallerThanExpected)
                .OrderBy(s => s.ValueDifference)
                .FirstOrDefault();

            var bestSuggestionGameDisplayNames = await GetGameDisplayNames(baseGameNames.ToList());

            if (bestSmallerSuggestion != null)
            {
                bestSmallerSuggestion.GameComparisons = AssignGameDisplayName(bestSmallerSuggestion.GameComparisons, bestSuggestionGameDisplayNames);
            }
            if (bestSuggestion != null)
            {
                bestSuggestion.GameComparisons = AssignGameDisplayName(bestSuggestion.GameComparisons, bestSuggestionGameDisplayNames);
            }

            return new BetLimitSuggestionResult
            {
                BestSuggestion = bestSuggestion,
                BestSmallerSuggestion = bestSmallerSuggestion
            };
        }

        private List<GameBetLimitComparison> AssignGameDisplayName(List<GameBetLimitComparison> gameComparisons, Dictionary<string, string> gameDisplayNames)
        {
            return gameComparisons.Select(x =>
            {
                x.GameDisplayName = gameDisplayNames[x.GameName];
                return x;
            }).ToList();
        }

        private async Task<Dictionary<string, string>> GetGameDisplayNames(List<string> gameNames)
        {
            var gameConfigs = await gameConfigRepository.FilterByAsync(x => gameNames.Contains(x.Name));
            return gameConfigs.ToDictionary(x => x.Name, x => x.DisplayName);
        }

        public BetLimitComparison CreateBetLimitComparison(double baseValue, double candidateValue, double rate)
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

        public BetLimitComparison CreateMinArrayBetLimitComparison(double[] baseValues, double[] candidateValues, double rate)
        {
            if (baseValues == null || candidateValues == null || baseValues.Length == 0 || candidateValues.Length == 0)
            {
                return new BetLimitComparison();
            }

            var baseMin = baseValues.Min();
            var candidateMin = candidateValues.Min();
            return CreateBetLimitComparison(baseMin, candidateMin, rate);
        }

        public BetLimitComparison CreateMaxArrayBetLimitComparison(double[] baseValues, double[] candidateValues, double rate)
        {
            if (baseValues == null || candidateValues == null || baseValues.Length == 0 || candidateValues.Length == 0)
            {
                return new BetLimitComparison();
            }

            var baseMax = baseValues.Max();
            var candidateMax = candidateValues.Max();
            return CreateBetLimitComparison(baseMax, candidateMax, rate);
        }

        public CurrencyGroupGame CreateAdjustedCurrencyGroupGame(CurrencyGroupGameEntity baseGame, double rate, List<int> availableChipValues)
        {
            var adjustedCoinValues = AdjustArrayValues(baseGame.CoinValues, rate);
            var adjustedBetLevels = AdjustArrayValues(baseGame.BetLevels, rate);
            var adjustedChips = AdjustChips(baseGame.Chips, rate, availableChipValues);

            return new CurrencyGroupGame(
                groupId: "",
                gameId: baseGame.GameId,
                gameName: baseGame.GameName,
                enabled: false,
                minbet: SmartRound(baseGame.MinBet / rate),
                maxbet: SmartRound(baseGame.MaxBet / rate),
                maxBetPerChoices: AdjustMaxBetPerChoices(baseGame.MaxBetPerChoices, rate),
                chips: adjustedChips,
                betLevels: adjustedBetLevels,
                stakeLevels: baseGame.StakeLevels,
                coinValues: adjustedCoinValues,
                maxWinPerRound: SmartRound(baseGame.MaxWinPerRound / rate),
                defaultCoinValue: adjustedCoinValues?.Length > 0 ? adjustedCoinValues.Min() : (double?)null,
                defaultBetLevel: adjustedBetLevels?.Length > 0 ? adjustedBetLevels.Min() : (double?)null
            );
        }

        public static string[] AdjustChips(string[] chips, double rate, List<int> availableChipValues)
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
                    return adjustedValue.ToString(CultureInfo.InvariantCulture);
                }
                return chipValue;
            }).ToArray();
        }

        public List<int> GetAvailableChipValues()
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

        public static double RoundToNearestChipValue(double value, List<int> availableChipValues)
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

        public static Dictionary<string, double> AdjustMaxBetPerChoices(Dictionary<string, double> maxBetPerChoices, double rate)
        {
            if (maxBetPerChoices == null)
            {
                return new Dictionary<string, double>();
            }

            return maxBetPerChoices.ToDictionary(
                kvp => kvp.Key,
                kvp => SmartRound(kvp.Value / rate)
            );
        }

        public static double[] AdjustArrayValues(double[] values, double rate)
        {
            if (values == null)
            {
                return new double[0];
            }

            return values.Select(value => SmartRound(value / rate)).ToArray();
        }

        public static double SmartRound(double value)
        {
            if (value == 0)
            {
                return 0;
            }

            var absValue = Math.Abs(value);
            var sign = Math.Sign(value);

            var magnitude = Math.Floor(Math.Log10(absValue));

            if (magnitude >= 3)
            {
                var roundTo = Math.Pow(10, magnitude);
                return sign * Math.Round(absValue / roundTo) * roundTo;
            }

            if (magnitude >= 0)
            {
                return Math.Round(value, 1);
            }

            var decimalPlaces = (int)Math.Abs(magnitude);
            return Math.Round(value, decimalPlaces);
        }
    }
}
