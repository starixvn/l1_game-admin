using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Chips;
using GamesAdmin.Site.Features.Chips.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.Chips.Handlers
{
    public class GetAllChipsHandler : ChipHandler, IRequestHandler<GetAllChipsRequest, IEnumerable<ChipModel>>
    {
        public GetAllChipsHandler(IMapper mapper, IChipApi api) : base(mapper, api)
        {
        }

        public Task<IEnumerable<ChipModel>> Handle(GetAllChipsRequest request, CancellationToken cancellationToken)
            => Api.GetAll();
    }
}