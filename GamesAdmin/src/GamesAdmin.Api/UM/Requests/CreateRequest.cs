using GamesAdmin.Core.Models.Um;
using MediatR;

namespace GamesAdmin.Api.UM.Requests
{
    public class CreateRequest : IRequest<bool>
    {
        public CreateRequest(EventSchedule eventSchedule)
        {
            this.eventSchedule = eventSchedule;
        }

        public EventSchedule eventSchedule { get; }
    }
}
