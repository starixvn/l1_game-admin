using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Database.Entities.Currencies;
using GamesAdmin.Database.Entities.Lobby;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Api.Lobby
{
    public static class LobbyLog
    {
        public static string BuildChangedLog(LobbyEntity oldEntity, LobbyEntity newEntity, List<CurrencyGroupEntity> currencyGroups, List<GeneralGameSetting> gameSettings)
        {
            var changedSettings = new List<string>();

            if (!newEntity.Name.Equals(oldEntity.Name))
            {
                changedSettings.Add($"{"Name"}: {oldEntity.Name} -> {newEntity.Name}\r\n");
            }

            if (HasChange(newEntity.CurrencyGroupIds, oldEntity.CurrencyGroupIds))
            {
                var oldCurrencyGroupNames = oldEntity.CurrencyGroupIds.Select(groupId => currencyGroups.FirstOrDefault(group => group.Id.ToString() == groupId)?.Name);
                var newCurrencyGroupNames = newEntity.CurrencyGroupIds.Select(groupId => currencyGroups.FirstOrDefault(group => group.Id.ToString() == groupId)?.Name);
                changedSettings.Add($"{"CurrencyGroupIds"}: {JsonConvert.SerializeObject(oldCurrencyGroupNames)} -> {JsonConvert.SerializeObject(newCurrencyGroupNames)}\r\n");
            }

            if (HasChange(newEntity.MiniPanelGameIds, oldEntity.MiniPanelGameIds))
            {
                var oldMiniPanelGames = oldEntity.MiniPanelGameIds.Select(gameId => GetGameDisplayName(gameId, gameSettings));
                var newMiniPanelGames = newEntity.MiniPanelGameIds.Select(gameId => GetGameDisplayName(gameId, gameSettings));
                changedSettings.Add($"{"MiniPanelGameIds"}: {JsonConvert.SerializeObject(oldMiniPanelGames)} -> {JsonConvert.SerializeObject(newMiniPanelGames)}\r\n");
            }

            if (HasChange(newEntity.Banners, oldEntity.Banners))
            {
                var oldBanners = oldEntity.Banners.Select(banner => new BannerLog { DisplayTime = banner.DisplayTime, GameName = GetGameDisplayName(banner.GameId, gameSettings) });
                var newBanners = newEntity.Banners.Select(banner => new BannerLog { DisplayTime = banner.DisplayTime, GameName = GetGameDisplayName(banner.GameId, gameSettings) });
                changedSettings.Add($"{"Banners"}: {JsonConvert.SerializeObject(oldBanners)} -> {JsonConvert.SerializeObject(newBanners)}\r\n");
            }

            if (HasChange(newEntity.RoundTimerGame, oldEntity.RoundTimerGame))
            {
                changedSettings.Add($"{"RoundTimerGame"}: {JsonConvert.SerializeObject(oldEntity.RoundTimerGame)} -> {JsonConvert.SerializeObject(newEntity.RoundTimerGame)}\r\n");
            }

            var changedCategory = BuildCategoryChangedLog(oldEntity, newEntity, gameSettings);
            changedSettings.Add(changedCategory);

            return string.Join("\r\n", changedSettings);
        }



        private static string BuildCategoryChangedLog(LobbyEntity oldEntity, LobbyEntity newEntity, List<GeneralGameSetting> gameSettings)
        {
            var changedSettings = new List<string>();
            var maxSize = oldEntity.Categories.Length > newEntity.Categories.Length ? oldEntity.Categories.Length : newEntity.Categories.Length;
            for (var i = 0; i < maxSize; i++)
            {
                var oldCategory = oldEntity.Categories.Length > i ? oldEntity.Categories[i] : null;
                var newCategory = newEntity.Categories.Length > i ? newEntity.Categories[i] : null;
                if (newCategory == null)
                {
                    changedSettings.Add($"- {oldCategory.Name.EN_US}: Deleted");
                }
                else if (oldCategory == null)
                {
                    changedSettings.Add($"- {newCategory.Name.EN_US}: Added");
                }
                else
                {
                    var changedCategory = new List<string>();
                    if (HasChange(newCategory.Name, oldCategory.Name))
                    {
                        changedCategory.Add($"{" - Name"}: {JsonConvert.SerializeObject(oldCategory.Name)} -> {JsonConvert.SerializeObject(newCategory.Name)}");
                    }
                    if (HasChange(newCategory.GameSettings, oldCategory.GameSettings))
                    {
                        var oldGameSettings = oldCategory.GameSettings.Select(gameSetting => new GameSettingLog { BigSize = gameSetting.BigSize, NewTag = gameSetting.NewTag, GameName = GetGameDisplayName(gameSetting.GameId, gameSettings)});
                        var newGameSettings = newCategory.GameSettings.Select(gameSetting => new GameSettingLog { BigSize = gameSetting.BigSize, NewTag = gameSetting.NewTag, GameName = GetGameDisplayName(gameSetting.GameId, gameSettings) });
                        changedCategory.Add($"{" - GameSettings"}: {JsonConvert.SerializeObject(oldGameSettings)} -> {JsonConvert.SerializeObject(newGameSettings)}");
                    }
                    if (changedCategory.Count > 0)
                    {
                        changedCategory.Insert(0, $"- {newCategory.Name.EN_US}:");
                        changedCategory.ForEach(category =>
                        {
                            changedSettings.Add(category);
                        });
                    }
                }
            }

            if (changedSettings.Count > 0)
            {
                changedSettings.Insert(0, "Categories:");
            }

            return string.Join("\r\n", changedSettings);
        }

        public static bool HasChange(object obj1, object obj2)
        {
            var obj1Str = JsonConvert.SerializeObject(obj1);
            var obj2Str = JsonConvert.SerializeObject(obj2);
            return !obj1Str.Equals(obj2Str);
        }

        private static string GetGameDisplayName(int gameId, List<GeneralGameSetting> gameSettings)
        {
            var gameSetting = gameSettings.FirstOrDefault(game => game.GameId == gameId);

            return gameSetting.DisplayName;
        }
    }
}
