using System.Threading.Tasks;
using Coravel.Invocable;

namespace GamesAdmin.Site.Features.Bot.ScheduleTasks
{
    public class ReviseBotBalance : IInvocable
    {
        private readonly IBotService service;

        public ReviseBotBalance(IBotService service)
        {
            this.service = service;
        }

        public Task Invoke() => service.ReviseBalance();
    }
}
