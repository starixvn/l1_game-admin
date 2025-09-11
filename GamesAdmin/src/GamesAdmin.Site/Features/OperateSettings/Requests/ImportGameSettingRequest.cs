using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.OperateSettings.Requests
{
    public class ImportGameSettingRequest
    {
        [Required]
        public IEnumerable<GameConfigImport> GameSettings { get; set; }
    }

    /// <summary>
    /// Extended GameConfig class for JSON deserialization with additional properties
    /// </summary>
    public class GameConfigImport : GameConfig
    {
        public GameConfigImport() : base(string.Empty, new Dictionary<string, double>())
        {
            // Default constructor for JSON deserialization
            // Calls base constructor with required parameters
        }
    }
}
