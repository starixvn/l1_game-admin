using System.Collections.Generic;

namespace GamesAdmin.Database.Entities.Keno
{
    public class JoinedAccountInfoEntity : AccountEntity
    {
        public IEnumerable<AccountInfoEntity> InnerUserInfo { get; set; }
    }
}
