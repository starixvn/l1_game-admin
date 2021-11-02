using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Partners.Requests;
using GamesAdmin.Core.Models.Partners;
using MediatR;

namespace GamesAdmin.Api.Partners
{
    public class PartnerHanlder
        : IRequestHandler<CreateRequest, bool>
        , IRequestHandler<DeleteRequest, bool>
        , IRequestHandler<GetAllRequest, IEnumerable<Partner>>
        , IRequestHandler<UpdateRequest, bool>
        , IRequestHandler<GetByIdRequest, Partner>
        , IRequestHandler<GetConfigRequest, PartnerConfig>
        , IRequestHandler<UpdateConfigRequest, bool>

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

        public Task<IEnumerable<Partner>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        => partnerService.GetAll();

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        => partnerService.Update(request.Partner);

        public Task<Partner> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        => partnerService.GetById(request.PartnerId);

        public Task<PartnerConfig> Handle(GetConfigRequest request, CancellationToken cancellationToken)
        => partnerService.GetConfig(request.PartnerId, request.Name);

        public Task<bool> Handle(UpdateConfigRequest request, CancellationToken cancellationToken)
        => partnerService.UpsertConfig(request.Config);
    }
}
