using System.Collections.Generic;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class ShootingBlitzTeam
    {
        public string Id { get; set; }

        public int TeamId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 是否啟用此 Team（預設 true，null 視為 true）
        /// </summary>
        public bool? IsEnabled { get; set; } = true;

        public List<ShootingBlitzPlayer> PlayerList { get; set; }
    }

    public class ShootingBlitzPlayer
    {
        public string Name { get; set; }

        public int PlayerNumber { get; set; }
    }
}

