using AutoMapper;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Chips;

namespace GamesAdmin.Api.Chips.Handlers
{
    public class ChipHandler
    {
        public ChipHandler(IMapper mapper, IGenericRepository<ChipEntity> repository)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public IGenericRepository<ChipEntity> Repository { get; }

        public IMapper Mapper { get; }
    }
}