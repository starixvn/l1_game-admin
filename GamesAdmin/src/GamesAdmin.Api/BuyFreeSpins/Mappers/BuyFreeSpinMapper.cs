using AutoMapper;
using GamesAdmin.Api.BuyFreeSpins.Requests;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.BuyFreeSpins.Mappers
{
    public class BuyFreeSpinMapper : Profile
    {
        public BuyFreeSpinMapper()
        {
            CreateMap<CreateBuyFreeSpinModel, CreateBuyFreeSpinRequest>()
                .ConstructUsing(model => new CreateBuyFreeSpinRequest(model.Enabled, model.Multiplier, model.SiteExclusions, model.GameId));

             CreateMap<UpdateBuyFreeSpinModel, UpdateBuyFreeSpinRequest>()
                 .ConstructUsing(model => new UpdateBuyFreeSpinRequest(model.Enabled, model.Multiplier, model.SiteExclusions, model.GameId));

            CreateMap<EnableBuyFreeSpinModel, EnableBuyFreeSpinRequest>()
                .ConstructUsing(model => new EnableBuyFreeSpinRequest(model.GameIds, model.IsEnabled));
        }
    }
}