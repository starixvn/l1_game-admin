using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateShootingBlitzConfigRequest : IRequest<bool>
    {
        public string Name { get; set; }

        public ShootingBlitzConfig ShootingBlitzConfig { get; set; }

        public GameInfo GameInfo { get; set; }

        public BotSetting BotSetting { get; set; }

        public EnableSetting EnableSetting { get; set; }
    }
}

