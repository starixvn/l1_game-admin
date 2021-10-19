using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroups.Requests;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroups.Handlers
{
    public class GetByIdHandler : Handler, IRequestHandler<GetByIdRequest, CurrencyGroupModel>
    {
        public GetByIdHandler(IMapper mapper, IGenericRepository<CurrencyGroupEntity> repository, ICurrencyService currency)
            : base(mapper, repository, currency)
        {
        }

        public async Task<CurrencyGroupModel> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            var group = await Repository.FindByIdAsync(request.Id);
            var currencies = await Currency.GetByGroup(group.Id.ToString());

            return TransformToModel(group, currencies);
        }
    }
}