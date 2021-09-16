using System.Collections.Generic;
using GamesAdmin.Core.Models.Um;
using MediatR;

namespace GamesAdmin.Api.UM.Requests
{
    public class GetUnfinishedUmRequest : IRequest<IList<UmSchedule>>
    {
    }
}