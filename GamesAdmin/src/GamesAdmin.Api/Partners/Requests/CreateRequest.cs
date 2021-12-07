using System;
using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.Partners;
using MediatR;

namespace GamesAdmin.Api.Partners.Requests
{
    public class CreateRequest : IRequest<bool>
    {
        public CreateRequest(Partner partner)
        {
            Partner = partner;
        }

        public Partner Partner { get; }
    }
}
