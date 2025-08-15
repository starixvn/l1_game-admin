using AutoMapper;

namespace GamesAdmin.Site.Features.BuyFreeSpins.Handlers
{
    public class BuyFreeSpinHandler
    {
        public BuyFreeSpinHandler(IMapper mapper, IBuyFreeSpinApi api)
        {
            Api = api;
            Mapper = mapper;
        }

        public IBuyFreeSpinApi Api { get; }

        public IMapper Mapper { get; }
    }
}