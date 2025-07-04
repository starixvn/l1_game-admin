using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.GameSettings;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class CreateWithSuggestionHandler : Handler, IRequestHandler<CreateWithSuggestionRequest, MessageResult>
    {
        private readonly IGlobalConfigApi globalConfigApi;

        public CreateWithSuggestionHandler(IMapper mapper, ICurrencyApi api, IGlobalConfigApi globalConfigApi) : base(mapper, api)
        {
            this.globalConfigApi = globalConfigApi;
        }

        public async Task<MessageResult> Handle(CreateWithSuggestionRequest request, CancellationToken cancellationToken)
        {
            var entities = await Api.GetAll();
            var model = Mapper.Map<CurrencyModel>(request.Model);
            var isExisted = entities.Any(entity => entity.PartnerId.Equals(model.PartnerId) && entity.Name.Equals(model.Name));
            var message = string.Empty;
            var success = false;
            var codeExisted = await IsExchangeCodeExisted(model.Code);

            if (isExisted)
            {
                message = $"Currency {model.Name} has existed. Please try another.";
            }
            else if (!codeExisted)
            {
                message = $"Code {model.Code} is invalid.";
            }
            else
            {
                var added = await Api.Create(model);

                if (added)
                {
                    success = true;
                    message = "Added successfully with suggestions.";
                }
                else
                {
                    message = "Cannot add new currency.";
                }
            }

            return new MessageResult(success, message);
        }

        private async Task<bool> IsExchangeCodeExisted(string exchangeCode)
        {
            var global = await globalConfigApi.GetExchangeCodes();

            global.Configs.TryGetValue("codes", out IList<string> codes);

            var codeExisted = codes != null && codes.Any(code => string.Equals(code, exchangeCode));

            return codeExisted;
        }
    }
} 