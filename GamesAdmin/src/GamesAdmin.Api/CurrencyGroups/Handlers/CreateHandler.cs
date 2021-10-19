using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroups.Requests;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroups.Handlers
{
    public class CreateHandler : Handler, IRequestHandler<CreateRequest, bool>
    {
        public CreateHandler(IMapper mapper, IGenericRepository<CurrencyGroupEntity> repository, ICurrencyService currency)
            : base(mapper, repository, currency)
        {
        }

        public async Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = Mapper.Map<CurrencyGroupEntity>(request.Model);

                await Repository.InsertOneAsync(entity);

                foreach (var currency in request.Model.Currencies)
                {
                    await Currency.ChangeCurrencyGroup(currency, entity.Id.ToString());
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}