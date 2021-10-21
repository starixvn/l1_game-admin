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

namespace GamesAdmin.Api.Chips.Handlers
{
    public class UpdateChipHandler : ChipHandler, IRequestHandler<UpdateChipRequest, bool>
    {
        public UpdateChipHandler(IMapper mapper, IGenericRepository<ChipEntity> repository) : base(mapper, repository)
        {
        }

        public async Task<bool> Handle(UpdateChipRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var update = Builders<ChipEntity>.Update
                    .Set(field => field.Text, request.Text)
                    .Set(field => field.Value, request.Value);

                await Repository.UpdateAsync(entity => entity.Id.Equals(ObjectId.Parse(request.Id)), update);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}