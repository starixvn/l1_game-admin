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
                await accountService.ReviseBotBalance();

                await Task.Delay(TimeSpan.FromMinutes(30), cancellationToken);
            }
        }
    }    
}
