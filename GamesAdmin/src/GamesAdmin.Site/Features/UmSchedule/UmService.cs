using System.Threading.Tasks;
using GamesAdmin.Core.Models.Um;
using GamesAdmin.Site.Features._Shared;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace GamesAdmin.Site.Features.UmSchedule
{
    public interface IUmApi : IBaseAuthorizationApi
    {
        [Post("/um/create")]
        Task<bool> Create(EventSchedule eventSchedule);
    }

    public interface IUmService
    {
        Task<bool> Add(EventSchedule schedule);
    }

    public class UmService : IUmService
    {
        private readonly IUmApi umApi;

        public UmService(IUmApi umApi) 
        {
            this.umApi = umApi;
        }

        public Task<bool> Add(EventSchedule schedule)
        => umApi.Create(schedule);
    }
}
