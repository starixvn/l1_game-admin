using MediatR;
using System;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class DisableAllRequest : IRequest<bool>
    {
        public DisableAllRequest(DateTimeOffset? disableAt)
        {
            DisableAt = disableAt ?? DateTimeOffset.Now;
        }

        public DateTimeOffset DisableAt { get; set; }
    }
}
