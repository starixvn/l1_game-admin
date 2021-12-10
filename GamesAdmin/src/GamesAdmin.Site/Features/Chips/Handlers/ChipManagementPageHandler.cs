using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Chips.Requests;
using GamesAdmin.Site.Features.Chips.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Chips.Handlers
{
    public class ChipManagementPageHandler : ChipHandler, IRequestHandler<ChipManagementPageRequest, ChipManagementViewModel>
    {
        public ChipManagementPageHandler(IMapper mapper, IChipApi api) : base(mapper, api)
        {
        }

        public async Task<ChipManagementViewModel> Handle(ChipManagementPageRequest request, CancellationToken cancellationToken)
        {
            var chips = await Api.GetAll();

            return new ChipManagementViewModel
            {
                Models = chips
            };
        }
    }
}