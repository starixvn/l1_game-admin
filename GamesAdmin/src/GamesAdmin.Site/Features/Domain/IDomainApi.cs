namespace GamesAdmin.Site.Features.Domain
{
    using GamesAdmin.Core.Models;
    using GamesAdmin.Site.Features._Shared;
    using Refit;
    using System.Threading.Tasks;

    public interface IDomainApi : IBaseAuthorizationApi
    {
        [Get("/domain/get")]
        Task<DomainModel> Get();

        [Post("/domain/update")]
        Task<bool> Update(DomainModel model);
    }
}
