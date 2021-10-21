using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies.Requests;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GamesAdmin.Api.Currencies.Handlers
{
    public class UpdateHandler : Handler, IRequestHandler<UpdateRequest, bool>
    {
        public UpdateHandler(IMapper mapper, IGenericRepository<CurrencyEntity> repository) : base(mapper, repository)
        {
        }

        public async Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Model;
                var update = Builders<CurrencyEntity>.Update
                    .Set(field => field.Name, model.Name)
                    .Set(field => field.Code, model.Code)
                    .Set(field => field.Multiplier, model.Multiplier)
                    .Set(field => field.Enabled, model.Enabled)
                    .Set(field => field.IsTest, model.IsTest)
                    .Set(field => field.PartnerId, model.PartnerId)
                    .Set(field => field.GroupId, model.GroupId);

                await Repository.UpdateAsync(entity => entity.Id.Equals(ObjectId.Parse(model.Id)), update);

                return true;
            } catch (Exception)
            {
                return false;
            }
        }
    }
}