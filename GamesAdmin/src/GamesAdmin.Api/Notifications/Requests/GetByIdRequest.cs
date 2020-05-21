using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Notifications.Requests
{
    public class GetByIdRequest : IRequest<Notification>
    {
    }
}
