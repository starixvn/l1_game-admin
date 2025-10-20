using AutoMapper;
using GamesAdmin.Database.Entities;
using GamesAdmin.Api.PlayerChainResults.ViewModels;

namespace GamesAdmin.Api.PlayerChainResults.Mappers
{
    public class PlayerChainResultMapper : Profile
    {
        public PlayerChainResultMapper()
        {
            CreateMap<PlayerChainAssignmentEntity, PlayerChainResultViewModel>();
        }
    }
}
