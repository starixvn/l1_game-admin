using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.CurrencyGroups.Models;
using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;

namespace GamesAdmin.Site.Features.CurrencyGroups.Mappers
{
    public class CurrencyGroupMapper : Profile
    {
        public CurrencyGroupMapper()
        {
            CreateMap<CreateCurrencyGroupViewModel, CreateCurrencyGroupModel>();
            CreateMap<UpdateCurrencyGroupViewModel, UpdateCurrencyGroupModel>();
            CreateMap<CurrencyGroupModel, CurrencyGroupViewModel>();
        }
    }
}