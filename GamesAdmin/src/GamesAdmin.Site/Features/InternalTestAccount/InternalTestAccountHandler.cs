namespace GamesAdmin.Site.Features.InternalTestAccount
{
    using ViewModels;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using GamesAdmin.Core.Models;

    public class InternalTestAccountHandler : IRequestHandler<GetInternalTestAccountRequest, IndexViewModel>, IRequestHandler<UpdateInternalTestAccountRequest, UpdateInternalUUSTestResult>
    {
        private readonly IInternalTestAccountService internalTestAccountService;


        public InternalTestAccountHandler(IInternalTestAccountService internalTestAccountService)
        {
            this.internalTestAccountService = internalTestAccountService;
        }

        public async Task<IndexViewModel> Handle(GetInternalTestAccountRequest request, CancellationToken cancellationToken)
        {
            var testAccounts = await internalTestAccountService.Get();
            return new IndexViewModel
            {
                CurrentAccountTest = string.Join(";", testAccounts),
                OriginalAccountTest = string.Join(";", testAccounts)
            };
        }


        public Task<UpdateInternalUUSTestResult> Handle(UpdateInternalTestAccountRequest request, CancellationToken cancellationToken)
        {         
            return internalTestAccountService.UpdateInternalTestAcount(request);
        }
  
    }
}
