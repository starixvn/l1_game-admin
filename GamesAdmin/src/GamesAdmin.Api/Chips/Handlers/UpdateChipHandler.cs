using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Chips.Requests;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Chips;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.Chips.Handlers
{
    public class UpdateChipHandler : ChipHandler, IRequestHandler<UpdateChipRequest, bool>
    {
        private readonly ISentryClient sentry;

        public UpdateChipHandler(IMapper mapper, IGenericRepository<ChipEntity> repository, ISentryClient sentry) : base(mapper, repository)
        {
            this.sentry = sentry;
        }

        public async Task<bool> Handle(UpdateChipRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var update = Builders<ChipEntity>.Update
                    .Set(field => field.Text, request.Text)
                    .Set(field => field.Value, request.Value)
                    .Set(field => field.Color, request.Color);

                await Repository.UpdateAsync(entity => entity.Id.Equals(ObjectId.Parse(request.Id)), update);

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