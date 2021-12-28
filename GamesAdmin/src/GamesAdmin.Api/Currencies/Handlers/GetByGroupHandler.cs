using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies.Requests;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using MediatR;
using MongoDB.Bson;
using Sentry;

namespace GamesAdmin.Api.Currencies.Handlers
{
    public class GetByGroupHandler : Handler, IRequestHandler<GetByGroupRequest, IEnumerable<CurrencyModel>>
    {
        public GetByGroupHandler(IMapper mapper, IGenericRepository<CurrencyEntity> repository, ISentryClient sentry) : base(mapper, repository, sentry)
        {
        }

        public async Task<IEnumerable<CurrencyModel>> Handle(GetByGroupRequest request, CancellationToken cancellationToken)
        {
            var entities = await Repository.FilterByAsync(entity => entity.GroupId == ObjectId.Parse(request.Model.GroupId));

            return entities.Select(entity => Mapper.Map<CurrencyModel>(entity)).ToList();
        }
    }
}