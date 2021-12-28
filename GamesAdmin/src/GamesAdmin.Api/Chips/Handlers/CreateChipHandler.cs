using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Chips.Requests;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Chips;
using MediatR;
using Sentry;

namespace GamesAdmin.Api.Chips.Handlers
{
    public class CreateChipHandler : ChipHandler, IRequestHandler<CreateChipRequest, bool>
    {
        private readonly ISentryClient sentry;

        public CreateChipHandler(IMapper mapper, IGenericRepository<ChipEntity> repository, ISentryClient sentry) : base(mapper, repository)
        {
            this.sentry = sentry;
        }

        public async Task<bool> Handle(CreateChipRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = Mapper.Map<ChipEntity>(request);

                await Repository.InsertOneAsync(entity);

                return true;
            }
            catch (Exception ex)
            {
                sentry.CaptureException(ex);

                return false;
            }
        }
    }
}