using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.ProVietKeno.Extensions
{
    public static class ProVietKenoExtensions
    {
        public static ProVietKenoRound ConvertToModel(this ProVietKenoRoundEntity entity)
        {
            return new ProVietKenoRound
            {
                Time = entity.Time,
                Result = entity.Result,
                Number = entity.Number,
                Big = entity.SettlementResult?.Big,
                Small = entity.SettlementResult?.Small,
                Odd = entity.SettlementResult?.Odd,
                Even = entity.SettlementResult?.Even,
                Earth = entity.SettlementResult?.Earth,
                Fire = entity.SettlementResult?.Fire,
                Gold = entity.SettlementResult?.Gold,
                Wood = entity.SettlementResult?.Wood,
                Water = entity.SettlementResult?.Water,
                Settled = entity.Settled
            };
        }
    }
}