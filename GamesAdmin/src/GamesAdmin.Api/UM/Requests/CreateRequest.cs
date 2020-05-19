using GamesAdmin.Core.Models.Um;
using MediatR;

namespace GamesAdmin.Api.UM.Requests
{
    public class CreateRequest : IRequest<bool>
    {
        public CreateRequest(UmSchedule eventSchedule)
        {
            EventSchedule = eventSchedule;
        }

        public UmSchedule EventSchedule { get; }
    }
}
