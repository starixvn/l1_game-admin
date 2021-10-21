using AutoMapper;
using GamesAdmin.Core.Models.Chips;
using GamesAdmin.Site.Features.Chips.Requests;
using GamesAdmin.Site.Features.Chips.ViewModels;

namespace GamesAdmin.Site.Features.Chips.Mappers
{
    public class ChipMapper : Profile
    {
        public ChipMapper()
        {
            CreateMap<CreateChipViewModel, CreateChipRequest>()
                .ConstructUsing(model => new CreateChipRequest(model.Text, model.Value));

            CreateMap<UpdateChipViewModel, UpdateChipRequest>()
                .ConstructUsing(model => new UpdateChipRequest(model.Id, model.Text, model.Value));

            CreateMap<ChipModel, UpdateChipViewModel>();

            CreateMap<CreateChipRequest, ChipModel>()
                .ConstructUsing(model => new ChipModel(string.Empty, model.Text, model.Value));

            CreateMap<UpdateChipRequest, ChipModel>()
                .ConstructUsing(model => new ChipModel(model.Id, model.Text, model.Value));
        }
    }
}