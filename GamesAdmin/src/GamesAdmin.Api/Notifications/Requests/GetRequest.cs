using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Notifications.Requests
{
    public class GetRequest: IRequest<IList<Notification>>
    {
    }
}
