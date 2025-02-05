namespace GamesAdmin.Api
{
    using AutoMapper;
    using GamesAdmin.Api.InternalTestAccount.Requests;
    using GamesAdmin.Core.Models;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class InternalTestAccountHandler : IRequestHandler<GetInternalTestAccountRequest, string[]>, IRequestHandler<UpdateInternalTestAccountRequest, UpdateInternalUUSTestResult>
    {
        private readonly IInternalTestAccountService internalTestAccountService;

        private readonly IMapper mapper;

        public InternalTestAccountHandler(IInternalTestAccountService internalTestAccountService, IMapper mapper)
        {
            this.internalTestAccountService = internalTestAccountService;
            this.mapper = mapper;
        }

        public Task<UpdateInternalUUSTestResult> Handle(UpdateInternalTestAccountRequest request, CancellationToken cancellationToken)
            => this.internalTestAccountService.UpdateInternalTest(request.AddAccounts, request.DeletedAccounts);
        
    

        public Task<string[]> Handle(GetInternalTestAccountRequest request, CancellationToken cancellationToken)
            => this.internalTestAccountService.Get();
    }
}
