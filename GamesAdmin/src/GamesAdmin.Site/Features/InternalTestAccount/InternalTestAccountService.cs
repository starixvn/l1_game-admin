namespace GamesAdmin.Site.Features.InternalTestAccount
{
    using System.Threading.Tasks;

    public interface IInternalTestAccountService
    {
        Task<string[]> Get();

        Task<bool> UpdateInternalTestAcount(UpdateInternalTestAccountRequest model);
    }

    public class InternalTestAccountService : IInternalTestAccountService
    {
        private readonly IInternalTestAccountApi internalTestAccountApi;

        public InternalTestAccountService(IInternalTestAccountApi internalTestAccountApi)
        {
            this.internalTestAccountApi = internalTestAccountApi;
        }

        public Task<bool> UpdateInternalTestAcount(UpdateInternalTestAccountRequest model)
           => internalTestAccountApi.Update(model);

        public Task<string[]> Get()
           => internalTestAccountApi.Get();
    }
}
