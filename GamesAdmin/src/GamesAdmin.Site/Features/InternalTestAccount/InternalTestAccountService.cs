namespace GamesAdmin.Site.Features.InternalTestAccount
{
    using System.Threading.Tasks;
    using GamesAdmin.Core.Models;

    public interface IInternalTestAccountService
    {
        Task<string[]> Get();

        Task<UpdateInternalUUSTestResult> UpdateInternalTestAcount(UpdateInternalTestAccountRequest model);
    }

    public class InternalTestAccountService : IInternalTestAccountService
    {
        private readonly IInternalTestAccountApi internalTestAccountApi;

        public InternalTestAccountService(IInternalTestAccountApi internalTestAccountApi)
        {
            this.internalTestAccountApi = internalTestAccountApi;
        }

        public Task<UpdateInternalUUSTestResult> UpdateInternalTestAcount(UpdateInternalTestAccountRequest model)
           => internalTestAccountApi.Update(model);

        public Task<string[]> Get()
           => internalTestAccountApi.Get();
    }
}
