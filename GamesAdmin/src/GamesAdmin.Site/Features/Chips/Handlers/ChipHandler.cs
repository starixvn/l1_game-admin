using AutoMapper;

namespace GamesAdmin.Site.Features.Chips.Handlers
{
    public class ChipHandler
    {
        public ChipHandler(IMapper mapper, IChipApi api)
        {
            Api = api;
            Mapper = mapper;
        }

        public IChipApi Api { get; }

        public IMapper Mapper { get; }
    }
}