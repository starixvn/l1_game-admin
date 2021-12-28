using AutoMapper;
using GamesAdmin.Api.Chips.Requests;
using GamesAdmin.Core.Models.Chips;
using GamesAdmin.Database.Entities.Chips;

namespace GamesAdmin.Api.Chips.Mappers
{
    public class ChipMapper : Profile
    {
        public ChipMapper()
        {
            CreateMap<ChipEntity, ChipModel>()
                .ConstructUsing(entity => new ChipModel(entity.Id.ToString(), entity.Text, entity.Value, entity.Color));
            CreateMap<CreateChipRequest, ChipEntity>();

            CreateMap<CreateChipModel, CreateChipRequest>()
                .ConstructUsing(model => new CreateChipRequest(model.Text, model.Value, model.Color));
            CreateMap<UpdateChipModel, UpdateChipRequest>()
                .ConstructUsing(model => new UpdateChipRequest(model.Id, model.Text, model.Value, model.Color));
        }
    }
}