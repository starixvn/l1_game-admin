using System.Threading.Tasks;
using Coravel.Invocable;

namespace GamesAdmin.Site.Features.Bot.ScheduleTasks
{
    public class DisableBot : IInvocable
    {
        private readonly IBotService service;

        public DisableBot(IBotService service)
        {
            this.service = service;
        }

        public Task Invoke() => service.DisableRandomly();
    }
}
