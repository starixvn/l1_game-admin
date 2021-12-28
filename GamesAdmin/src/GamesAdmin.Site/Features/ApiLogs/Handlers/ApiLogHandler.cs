using AutoMapper;

namespace GamesAdmin.Site.Features.ApiLogs.Handlers
{
    public abstract class ApiLogHandler
    {
        protected ApiLogHandler(ILogApi api, IMapper mapper)
        {
            Api = api;
            Mapper = mapper;
        }

        protected ILogApi Api { get; }

        protected IMapper Mapper { get; }
    }
}