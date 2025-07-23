using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface ICascadingSlotsConfigValidator
    {
        string ValidateOdds(Dictionary<string, List<OddsByCount>> icons);
    }

    public class CascadingSlotsConfigValidator : ICascadingSlotsConfigValidator
    {
        public string ValidateOdds(Dictionary<string, List<OddsByCount>> icons)
        {
            for (var i = 0; i < icons.Count; i++)
            {
                var icon = icons[i.ToString()];
                var result = ValidateIconOdds(icon, i);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    return result;
                }
            }

            return string.Empty;
        }

        private string ValidateIconOdds(List<OddsByCount> icon, int iconIndex)
        {
            for (var i = 0; i < icon.Count; i++)
            {
                var odds = icon[i];

                if (!IsValidPositives(odds))
                {
                    return $"Icon {iconIndex} - Range {i}: The input value should be a possible number";
                }

                if (!IsValidFromTo(odds))
                {
                    return $"Icon {iconIndex} - Range {i}: The \"To\" value has to be greater than the \"From\" value";
                }

                if (i == 0)
                {
                    continue;
                }

                var prevOdds = icon[i - 1];
                if (!IsValidConsecutiveRange(odds, prevOdds))
                {
                    return $"Icon {iconIndex} - Range {i}: The input value has to be greater than the \"To\" value of the previous range one unit";
                }
            }

            return string.Empty;
        }

        private bool IsValidPositives(OddsByCount odds)
            => odds.From >= 0 && odds.To >= 0 && odds.Odds >= 0;

        private bool IsValidFromTo(OddsByCount odds)
            => odds.From <= odds.To;

        private bool IsValidConsecutiveRange(OddsByCount odds, OddsByCount prevOdds)
            => odds.From == prevOdds.To + 1;
    }
}
