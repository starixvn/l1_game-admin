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
        , IRequestHandler<ReviseBotBalanceRequest, int>
        , IRequestHandler<DisableBotRandomlyRequest, int>
    {
        private readonly IAccountService accountService;
        private readonly IBotService botService;

        public AccountHandler(IAccountService accountService, IBotService botService) 
        {
            this.accountService = accountService;
            this.botService = botService;
        }

        public Task<int> Handle(GetBotCountRequest request, CancellationToken cancellationToken)
        => accountService.GetTotalBots();

        public Task<IList<GenerateAccountResult>> Handle(GenerateAccountRandomRequest request, CancellationToken cancellationToken)
        => accountService.GenerateUsers(request.Names, request.IsBot);

        public Task<int> Handle(ReviseBotNameRequest request, CancellationToken cancellationToken)
        => accountService.ReviseBotNames();

        public Task<int> Handle(ReviseBotBalanceRequest request, CancellationToken cancellationToken)
        => botService.ReviseBalance();

        public Task<int> Handle(DisableBotRandomlyRequest request, CancellationToken cancellationToken)
        => botService.DisableRandomly();
    }
}
