using System;

namespace GamesAdmin.Database.Entities
{
    public abstract class MiniGameBaseEntity : Document
    {
        public DateTime time { get; set; }

        public int __v { get; set; }
    }
}