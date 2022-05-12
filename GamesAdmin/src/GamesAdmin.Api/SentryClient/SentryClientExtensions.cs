using System.Collections.Generic;
using System.Linq;
using Sentry;

namespace GamesAdmin.Api.SentryClient
{
    public static class SentryClientExtensions
    {
        private static string MessageChangedSetting = "CHANGE {0}: {1} has new {2}!!!{3} Details: {4}";

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

            sentryClient.CaptureMessage(message);
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
                string.Join("; ", changedSettings));

            sentryClient.CaptureMessage(message);
        }
    }
}
