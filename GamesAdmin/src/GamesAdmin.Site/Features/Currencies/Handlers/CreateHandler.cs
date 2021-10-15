using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Currencies.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class CreateHandler : Handler, IRequestHandler<CreateRequest, MessageResult>
    {
        public CreateHandler(IMapper mapper, ICurrencyApi api) : base(mapper, api)
        {
        }

        public async Task<MessageResult> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            var entities = await Api.GetAll();
            var model = Mapper.Map<CurrencyModel>(request.Model);
            var isExisted = entities.Any(entity => entity.PartnerId.Equals(model.PartnerId) && entity.Name.Equals(model.Name));
            var message = string.Empty;
            var success = false;

            if (isExisted)
            {
                message = $"Currency {model.Name} has existed. Please try another.";
            }
            else
            {
                var added = await Api.Create(model);

                if (added)
                {
                    success = true;
                    message = "Added successfully.";
                }
                else
                {
                    message = "Cannot add new currency.";
                }
            }

            return new MessageResult(success, message);
        }
    }
}