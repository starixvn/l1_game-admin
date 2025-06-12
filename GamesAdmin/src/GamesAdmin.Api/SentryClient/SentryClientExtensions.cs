using System.Collections.Generic;
using System.Linq;
using Sentry;
using Sentry.Protocol;
using Newtonsoft.Json;

namespace GamesAdmin.Api.SentryClient
{
    public static class SentryClientExtensions
    {
        private static string MessageChangedSetting = "CHANGE {0}: {1} has new {2}!!!{3} Details: {4}";
        private static string AddedSettingMessage = "ADD {0}: {1} Details: {2}";

        public static void CaptureAddSetting(
            this ISentryClient sentryClient,
            string gameName,
            string settingName,
            object addedSetting)
        {
            if (addedSetting == null)
            {
                return;
            }

            var message = string.Format(
                AddedSettingMessage,
                settingName.ToUpperInvariant(),
                gameName,
                JsonConvert.SerializeObject(addedSetting));

            sentryClient.CaptureMessage(message, SentryLevel.Error);
        }

        public static void CaptureChangedSetting(
            this ISentryClient sentryClient,
            string gameName,
            string settingName,
            string additionalDetails,
            SettingChangedModel setting)
        {
            if (setting.OldValue == setting.NewValue)
            {
                return;
            }

            var message = string.Format(
                MessageChangedSetting,
                settingName.ToUpperInvariant(),
                gameName,
                settingName,
                additionalDetails,
                $"{setting.OldValue} -> {setting.NewValue}");

            sentryClient.CaptureMessage(message, SentryLevel.Error);
        }

        public static void CaptureChangedSettings(
            this ISentryClient sentryClient,
            string gameName,
            string settingName,
            string additionalDetails,
            List<SettingChangedModel> settings)
        {
            var changedSettings = settings
                .Where(setting => setting.OldValue != setting.NewValue)
                .Select(setting => $"{setting.Name}: {setting.OldValue} -> {setting.NewValue}");

            if (!changedSettings.Any())
            {
                return;
            }

            var message = string.Format(
                MessageChangedSetting,
                settingName.ToUpperInvariant(),
                gameName,
                settingName,
                additionalDetails,
                string.Join(";\r\n", changedSettings));

            sentryClient.CaptureMessage(message, SentryLevel.Error);
        }
    }
}
