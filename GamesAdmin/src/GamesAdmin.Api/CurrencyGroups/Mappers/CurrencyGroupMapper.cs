using AutoMapper;
using GamesAdmin.Api.CurrencyGroups.Requests;
using GamesAdmin.Database.Entities.Currencies;

namespace GamesAdmin.Api.CurrencyGroups.Mappers
{
    public class CurrencyGroupMapper : Profile
    {
        public CurrencyGroupMapper()
        {
            CreateMap<CreateCurrencyGroupModel, CurrencyGroupEntity>();
        }
    }
}