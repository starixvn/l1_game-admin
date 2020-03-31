using GamesAdmin.Site.Features.Account.Requests;
using GamesAdmin.Site.Features.Account.Results;
using GamesAdmin.Site.Features.Account.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Account
{
    public class AccountHandler
        : IRequestHandler<AddRequest, AddViewModel>
        , IRequestHandler<AddUsersRequest, List<AddUsersResult>>
        , IRequestHandler<ReviseRequest, int>
    {
        private readonly IAccountService accountService;

        public AccountHandler(IAccountService accountService) 
        {
            this.accountService = accountService;
        }

        public Task<AddViewModel> Handle(AddRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new AddViewModel());

        public Task<List<AddUsersResult>> Handle(AddUsersRequest request, CancellationToken cancellationToken)
        {
            var names = request.TextNames?
                .Split(';')
                .Select(name => name.Trim())
                .ToArray();

            if (names?.Length > 0)
            {
                return accountService.AddUsers(names, request.IsBot); 
            }

            return Task.FromResult(new List<AddUsersResult>());
        }

        public Task<int> Handle(ReviseRequest request, CancellationToken cancellationToken)
        => accountService.ReviseBots(); 
    }
}
