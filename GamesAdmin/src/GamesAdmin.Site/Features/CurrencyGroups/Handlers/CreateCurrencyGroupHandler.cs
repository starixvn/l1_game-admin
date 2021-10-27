using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.CurrencyGroups.Models;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Handlers
{
    public class CreateCurrencyGroupHandler : Handler, IRequestHandler<CreateCurrencyGroupRequest, MessageResult>
    {
        public CreateCurrencyGroupHandler(IMapper mapper, ICurrencyGroupApi api) : base(mapper, api)
        {
        }

        public async Task<MessageResult> Handle(CreateCurrencyGroupRequest request, CancellationToken cancellationToken)
        {
            var model = Mapper.Map<CreateCurrencyGroupModel>(request.Model);

            if (!model.Currencies.Any())
            {
                const string errorMessage = "The Currency field is required.";

                return new MessageResult(false, errorMessage);
            }

            var added = await Api.Create(model);
            var message = added ? "Added successfully." : "Cannot add new currency group.";

            return new MessageResult(added, message);
        }
    }
}