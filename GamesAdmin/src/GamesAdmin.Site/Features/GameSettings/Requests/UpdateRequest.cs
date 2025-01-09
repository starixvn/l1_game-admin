using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateRequest : IRequest<bool>
    {
        public UpdateRequest(
            GameInfo gameInfo,
            BotSetting botSetting,
            EnableSetting enableSetting,
            SlotsConfig slotsConfig,
            CascadingSlotsConfig cascadingSlotsConfig,
            SlotRNGConfig slotRNGConfig,
            CrashConfig crashConfig,
            OddsMultipliersConfig oddsMultipliers,
            TournamentConfig tournamentConfig,
            VideoStreamConfig videoStreamConfig,
            IEnumerable<int> otherGameIdsInGroup)
        {
            GameInfo = gameInfo;
            BotSetting = botSetting;
            EnableSetting = enableSetting;
            SlotsConfig = slotsConfig;
            CascadingSlotsConfig = cascadingSlotsConfig;
            SlotRNGConfig = slotRNGConfig;
            CrashConfig = crashConfig;
            OddsMultipliers = oddsMultipliers;
            TournamentConfig = tournamentConfig;
            VideoStreamConfig = videoStreamConfig;
            OtherGameIdsInGroup = otherGameIdsInGroup ?? Enumerable.Empty<int>();
        }

        public GameInfo GameInfo { get; }

        public BotSetting BotSetting { get; }

        public EnableSetting EnableSetting { get; }

        public SlotsConfig SlotsConfig { get; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; }

        public SlotRNGConfig SlotRNGConfig { get; }

        public CrashConfig CrashConfig { get; }

        public OddsMultipliersConfig OddsMultipliers { get; set; }

		public TournamentConfig TournamentConfig { get; set; }

		public VideoStreamConfig VideoStreamConfig { get; set; }
		public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}