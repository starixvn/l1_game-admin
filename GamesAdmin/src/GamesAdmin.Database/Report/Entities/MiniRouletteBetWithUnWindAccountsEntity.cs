using GamesAdmin.Database.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GamesAdmin.Database.Report.Entities
{
    public class MiniRouletteBetWithUnWindAccountsEntity : MiniRouletteBetEntity
    {
        public ObjectId UserId { get; set; }

        public AccountInfoEntity AccountInfos { get; set; }

        public AccountEntity Accounts { get; set; }
    }
}
