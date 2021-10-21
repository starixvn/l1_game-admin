using AutoMapper;
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
        }
    }
}