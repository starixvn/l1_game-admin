using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Partner.Requests;
using GamesAdmin.Core.Models.Partner;
using MediatR;

namespace GamesAdmin.Api.Partner
{
    public class PartnerHanlder
        : IRequestHandler<CreateRequest, bool>
        , IRequestHandler<DeleteRequest, bool>
        , IRequestHandler<GetAllRequest, IEnumerable<PartnerInfo>>
        , IRequestHandler<UpdateRequest, bool>

    {
        private readonly IPartnerService partnerService;

        public PartnerHanlder(IPartnerService partnerService)
        {
            this.partnerService = partnerService;
        }

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => partnerService.Create(request.Partner);

        public Task<bool> Handle(DeleteRequest request, CancellationToken cancellationToken)
        => partnerService.Delete(request.PartnerId);

        public Task<IEnumerable<PartnerInfo>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        => partnerService.GetAll();

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        => partnerService.Update(request.Partner);

    }
}
