using System.Threading.Tasks;
using Coravel.Invocable;

namespace GamesAdmin.Site.Features.Bot.ScheduleTasks
{
    public class BanBot : IInvocable
    {
        private readonly IBotService service;

        public BanBot(IBotService service)
        {
            this.service = service;
        }

        public Task Invoke() => service.BanRandomly();
    }
}
