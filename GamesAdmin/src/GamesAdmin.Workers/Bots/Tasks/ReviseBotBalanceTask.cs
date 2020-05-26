using System.Threading.Tasks;
using Coravel.Invocable;

namespace GamesAdmin.Workers.Bots.Tasks
{
    public class ReviseBotBalanceTask : IInvocable
    {
        private readonly IBotService service;

        public ReviseBotBalanceTask(IBotService service) 
        {
            this.service = service;
        }

        public Task Invoke() => service.ReviseBalance();
    }
}
