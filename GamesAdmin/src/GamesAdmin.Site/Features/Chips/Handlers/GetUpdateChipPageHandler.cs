using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Chips.Requests;
using GamesAdmin.Site.Features.Chips.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Chips.Handlers
{
    public class GetUpdateChipPageHandler : ChipHandler, IRequestHandler<GetUpdateChipPageRequest, UpdateChipViewModel>
    {
        public GetUpdateChipPageHandler(IMapper mapper, IChipApi api) : base(mapper, api)
        {
        }

        public async Task<UpdateChipViewModel> Handle(GetUpdateChipPageRequest request, CancellationToken cancellationToken)
        {
            var chip = await Api.GetById(request.Id);

            return Mapper.Map<UpdateChipViewModel>(chip);
        }
    }
}