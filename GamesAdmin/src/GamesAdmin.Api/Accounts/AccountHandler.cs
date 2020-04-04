using GamesAdmin.Api.Accounts.Requests;
using GamesAdmin.Api.Accounts.Results;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Accounts
{
    public class AccountHandler
        : IRequestHandler<GetBotCountRequest, int>
        , IRequestHandler<GenerateAccountRandomRequest, IList<GenerateAccountResult>>
        , IRequestHandler<ReviseBotNameRequest, int>
    {
        private readonly IAccountService accountService;

        public AccountHandler(IAccountService accountService) 
        {
            this.accountService = accountService;
        }

        public Task<int> Handle(GetBotCountRequest request, CancellationToken cancellationToken)
        => accountService.GetTotalBots();

        public Task<IList<GenerateAccountResult>> Handle(GenerateAccountRandomRequest request, CancellationToken cancellationToken)
        => accountService.GenerateUsers(request.Names, request.IsBot);

        public Task<int> Handle(ReviseBotNameRequest request, CancellationToken cancellationToken)
        => accountService.ReviseBotNames();
    }
}
