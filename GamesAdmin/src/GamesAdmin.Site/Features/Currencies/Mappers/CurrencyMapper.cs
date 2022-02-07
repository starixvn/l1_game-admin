using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.Mappers
{
    public class CurrencyMapper : Profile
    {
        public CurrencyMapper()
        {
            CreateMap<CreateCurrencyViewModel, CurrencyModel>()
                 .ConstructUsing(viewModel => new CurrencyModel(
                     string.Empty,
                     viewModel.Name,
                     viewModel.Code,
                     viewModel.Multiplier,
                     viewModel.Enabled,
                     viewModel.IsTest,
                     viewModel.PartnerId,
                     viewModel.GroupId));

            CreateMap<UpdateCurrencyViewModel, CurrencyModel>()
                 .ConstructUsing(viewModel => new CurrencyModel(
                     viewModel.Id,
                     viewModel.Name,
                     viewModel.Code,
                     viewModel.Multiplier,
                     viewModel.Enabled,
                     viewModel.IsTest,
                     viewModel.PartnerId,
                     viewModel.GroupId));

            CreateMap<CurrencyModel, UpdateCurrencyViewModel>();

            CreateMap<CurrencyModel, SelectListItem>()
                .ConstructUsing(model => new SelectListItem($"{model.Name} ({model.Code})", model.Code));
        }
    }
}