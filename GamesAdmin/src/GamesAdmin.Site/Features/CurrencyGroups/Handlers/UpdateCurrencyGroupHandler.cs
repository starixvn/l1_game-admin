using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.CurrencyGroups.Models;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Handlers
{
    public class UpdateCurrencyGroupHandler : Handler, IRequestHandler<UpdateCurrencyGroupRequest, MessageResult>
    {
        public UpdateCurrencyGroupHandler(IMapper mapper, ICurrencyGroupApi api) : base(mapper, api)
        {
        }

        public async Task<MessageResult> Handle(UpdateCurrencyGroupRequest request, CancellationToken cancellationToken)
        {
            var model = Mapper.Map<UpdateCurrencyGroupModel>(request.Model);
            var added = await Api.Update(model);
            var message = added ? "Updated successfully." : "Cannot update currency group.";

            return new MessageResult(added, message);
        }
    }
}