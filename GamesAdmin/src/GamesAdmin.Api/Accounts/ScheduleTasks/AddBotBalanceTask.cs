using GamesAdmin.Api._Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Accounts.ScheduleTasks
{
    public class AddBotBalanceTask :  HostedService
    {
        private readonly IAccountService accountService;

        public AddBotBalanceTask(IAccountService accountService) 
        {
            this.accountService = accountService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await accountService.AddBotBalance();

                await Task.Delay(TimeSpan.FromHours(2), cancellationToken);
            }
        }
    }    
}
