using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Chips.Requests;
using GamesAdmin.Core.Models.Chips;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Chips;
using MediatR;

namespace GamesAdmin.Api.Chips.Handlers
{
    public class GetAllChipsHandler : ChipHandler, IRequestHandler<GetAllChipsRequest, IEnumerable<ChipModel>>
    {
        public GetAllChipsHandler(IMapper mapper, IGenericRepository<ChipEntity> repository) : base(mapper, repository)
        {
        }

        public async Task<IEnumerable<ChipModel>> Handle(GetAllChipsRequest request, CancellationToken cancellationToken)
        {
            var entities = await Repository.FilterByAsync(entity => true);

            return Mapper.Map<IEnumerable<ChipModel>>(entities.AsEnumerable());
        }
    }
}