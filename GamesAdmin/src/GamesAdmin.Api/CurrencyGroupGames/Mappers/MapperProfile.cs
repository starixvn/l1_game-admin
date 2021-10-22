using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.CurrencyGroupGames.Mappers
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CurrencyGroupGameEntity, CurrencyGroupGame>();

            CreateMap<CurrencyGroupGame, CurrencyGroupGameEntity>();
        }
    }
}