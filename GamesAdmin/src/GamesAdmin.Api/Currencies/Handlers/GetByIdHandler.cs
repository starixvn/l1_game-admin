using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies.Requests;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using MediatR;

namespace GamesAdmin.Api.Currencies.Handlers
{
    public class GetByIdHandler : Handler, IRequestHandler<GetByIdRequest, CurrencyModel>
    {
        public GetByIdHandler(IMapper mapper, IGenericRepository<CurrencyEntity> repository) : base(mapper, repository)
        {
        }

        public async Task<CurrencyModel> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            var entity = await Repository.FindByIdAsync(request.Id);

            return Mapper.Map<CurrencyModel>(entity);
        }
    }
}