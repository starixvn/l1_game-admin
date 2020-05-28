using System.Threading.Tasks;
using Coravel.Invocable;

namespace GamesAdmin.Workers.Bots.Tasks
{
    public class DisableBotTask : IInvocable
    {
        private readonly IBotService service;

        public DisableBotTask(IBotService service)
        {
            this.service = service;
        }

        public Task Invoke() => service.DisableRandomly();
    }
}
