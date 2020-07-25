using System.Collections.Generic;

namespace GamesAdmin.Database.Entities.Keno
{
    public class JoinedKenoBetOnes : KenoProVietOnesEntity
    {
        public IEnumerable<AccountEntity> InnerUsers { get; set; }
    }

    public class JoinedAccountInfoEntity : AccountEntity
    {
        public IEnumerable<AccountInfoEntity> InnerUserInfo { get; set; }
    }
}
