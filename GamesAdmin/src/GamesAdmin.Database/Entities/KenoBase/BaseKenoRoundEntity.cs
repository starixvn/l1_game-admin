using System.Collections.Generic;

namespace GamesAdmin.Database.Entities.Keno
{
    public abstract class BaseKenoRoundEntity : BaseRoundEntity
    {
        public IEnumerable<byte> Result { get; set; }
    }
}
