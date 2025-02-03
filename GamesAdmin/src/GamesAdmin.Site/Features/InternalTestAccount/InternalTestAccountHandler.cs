namespace GamesAdmin.Site.Features.InternalTestAccount
{
    using ViewModels;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using System;
    using System.Linq;

    public class InternalTestAccountHandler : IRequestHandler<GetInternalTestAccountRequest, IndexViewModel>, IRequestHandler<UpdateInternalTestAccountRequest, bool>
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


        public Task<bool> Handle(UpdateInternalTestAccountRequest request, CancellationToken cancellationToken)
        {         
            return internalTestAccountService.UpdateInternalTestAcount(request);
        }
  
    }
}
