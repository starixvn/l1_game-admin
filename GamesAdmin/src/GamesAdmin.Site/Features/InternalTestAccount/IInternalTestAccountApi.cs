namespace GamesAdmin.Site.Features.InternalTestAccount
{
    using GamesAdmin.Site.Features._Shared;
    using Refit;
    using System.Threading.Tasks;

    public interface IInternalTestAccountApi : IBaseAuthorizationApi
    {
        [Get("/internalTestAccount/get")]
        Task<string[]> Get();

        [Post("/internalTestAccount/update")]
        Task<bool> Update(UpdateInternalTestAccountRequest model);
    }
}
