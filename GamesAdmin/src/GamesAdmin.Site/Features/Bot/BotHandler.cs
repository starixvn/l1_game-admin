using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.Bot.Requests;
using GamesAdmin.Site.Features.Bot.Results;
using GamesAdmin.Site.Features.Bot.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Bot
{
    public class BotHandler
        : IRequestHandler<AddRequest, AddViewModel>
        , IRequestHandler<AddUsersRequest, List<AddBotResult>>
        , IRequestHandler<DeactivateRequest, int>
    {
        private readonly IBotService botService;

        public BotHandler(IBotService botService)
        {
            this.botService = botService;
        }

        public Task<AddViewModel> Handle(AddRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new AddViewModel());

        public Task<List<AddBotResult>> Handle(AddUsersRequest request, CancellationToken cancellationToken)
        {
            var names = request.TextNames?
                .Split(';')
                .Select(name => name.Trim())
                .ToArray();

            return (names?.Length > 0) ? botService.Add(names) : Task.FromResult(new List<AddBotResult>());
        }

        public Task<int> Handle(DeactivateRequest request, CancellationToken cancellationToken)
        => botService.Deactivate();
    }
}
