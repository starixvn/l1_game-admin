using System.Threading.Tasks;
using Coravel.Invocable;

namespace GamesAdmin.Workers.Bots.Tasks
{
    public class ReviseBalanceTask : IInvocable
    {
        private readonly IBotService service;

        public ReviseBalanceTask(IBotService service) 
        {
            this.service = service;
        }

        public Task Invoke() => service.ReviseBalance();
    }
}
