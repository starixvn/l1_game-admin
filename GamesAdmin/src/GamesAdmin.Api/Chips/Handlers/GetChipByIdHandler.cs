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
    public class GetChipByIdHandler : ChipHandler, IRequestHandler<GetChipByIdRequest, ChipModel>
    {
        public GetChipByIdHandler(IMapper mapper, IGenericRepository<ChipEntity> repository) : base(mapper, repository)
        {
        }

        public async Task<ChipModel> Handle(GetChipByIdRequest request, CancellationToken cancellationToken)
        {
            var entity = await Repository.FindByIdAsync(request.Id);

            return Mapper.Map<ChipModel>(entity);
        }
    }
}