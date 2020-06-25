using System.Collections.Generic;

namespace GamesAdmin.Database.Entities.Keno
{
    public class JoinedKenoBetOnes : KenoProVietOnesEntity
    {
        public IEnumerable<AccountEntity> InnerUsers { get; set; }
    }
}
