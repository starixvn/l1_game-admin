using System;
using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.Partner;
using MediatR;

namespace GamesAdmin.Api.Partner.Requests
{
    public class CreateRequest : IRequest<bool>
    {
        public CreateRequest(PartnerInfo partner)
        {
            Partner = partner;
        }

        public PartnerInfo Partner { get; }
    }
}
