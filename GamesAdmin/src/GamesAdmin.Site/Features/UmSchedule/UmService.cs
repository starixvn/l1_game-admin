using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.UmSchedule
{
    public interface IUmApi : IBaseAuthorizationApi
    {
        [Post("/um/add")]
        Task<bool> Add([Body] Core.Models.Um.UmSchedule eventSchedule);

        [Get("/um/all")]
        Task<IList<Core.Models.Um.UmSchedule>> GetAll();
    }

    public interface IUmService
    {
        Task<bool> Add(Core.Models.Um.UmSchedule schedule);

        Task<IList<Core.Models.Um.UmSchedule>> GetAll();
    }

    public class UmService : IUmService
    {
        private readonly IUmApi umApi;

        public UmService(IUmApi umApi)
        {
            this.umApi = umApi;
        }

        public Task<bool> Add(Core.Models.Um.UmSchedule schedule)
        => umApi.Add(schedule);

        public Task<IList<Core.Models.Um.UmSchedule>> GetAll()
        => umApi.GetAll();
    }
}
