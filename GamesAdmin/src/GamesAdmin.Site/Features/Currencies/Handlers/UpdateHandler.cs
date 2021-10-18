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
    public class UpdateHandler : Handler, IRequestHandler<UpdateRequest, MessageResult>
    {
        public UpdateHandler(IMapper mapper, ICurrencyApi api) : base(mapper, api)
        {
        }

        public async Task<MessageResult> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            var allCurrencies = await Api.GetAll();
            var currencies = allCurrencies.Where(entity => !entity.Id.Equals(request.Model.Id));
            var model = Mapper.Map<CurrencyModel>(request.Model);
            var isExisted = currencies.Any(entity => entity.PartnerId.Equals(model.PartnerId) && entity.Name.Equals(model.Name));
            var message = string.Empty;
            var success = false;

            if (isExisted)
            {
                message = $"Currency {model.Name} has existed. Please try another.";
            }
            else
            {
                var updated = await Api.Update(model);

                if (updated)
                {
                    success = true;
                    message = "Updated successfully.";
                }
                else
                {
                    message = "Updated failure.";
                }
            }

            return new MessageResult(success, message);
        }
    }
}