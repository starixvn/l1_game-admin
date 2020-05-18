using System;
using MediatR;

namespace GamesAdmin.Site.Features.UmSchedule.Requests
{
    public class CreateRequest : IRequest<bool>
    {       
        public DateTime Start { get; set; }
      
        public DateTime End { get; set; }

        public string Message { get; set; }
    }
}
