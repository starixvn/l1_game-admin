using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.Currencies.ViewModels;

namespace GamesAdmin.Site.Features.Currencies.Mappers
{
    public class CurrencyMapper : Profile
    {
        public CurrencyMapper()
        {
            CreateMap<CreateCurrencyViewModel, CurrencyModel>();
        }
    }
}