using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Chips;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Chips.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.Chips.Handlers
{
    public class CreateChipHandler : ChipHandler, IRequestHandler<CreateChipRequest, ResultBase>
    {
        public CreateChipHandler(IMapper mapper, IChipApi api) : base(mapper, api)
        {
        }

        public async Task<ResultBase> Handle(CreateChipRequest request, CancellationToken cancellationToken)
        {
            var result = await Api.Create(Mapper.Map<ChipModel>(request));

            if (result)
            {
                return new ResultBase(string.Empty);
            }
            else
            {
                return new ResultBase("Cannot create new chip.");
            }
        }
    }
}