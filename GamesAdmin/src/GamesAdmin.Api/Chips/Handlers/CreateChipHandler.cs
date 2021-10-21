using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Chips.Requests;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Chips;
using MediatR;

namespace GamesAdmin.Api.Chips.Handlers
{
    public class CreateChipHandler : ChipHandler, IRequestHandler<CreateChipRequest, bool>
    {
        public CreateChipHandler(IMapper mapper, IGenericRepository<ChipEntity> repository) : base(mapper, repository)
        {
        }

        public async Task<bool> Handle(CreateChipRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = Mapper.Map<ChipEntity>(request);

                await Repository.InsertOneAsync(entity);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}