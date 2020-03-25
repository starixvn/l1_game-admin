using GamesAdmin.Api._Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Users.ScheduleTasks
{
    public class AddBotBalanceTask :  HostedService
    {
        private readonly IUserService userService;

        public AddBotBalanceTask(IUserService userService) 
        {
            this.userService = userService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await userService.AddBotBalance();

                await Task.Delay(TimeSpan.FromHours(2), cancellationToken);
            }
        }
    }    
}
