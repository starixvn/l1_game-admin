namespace GamesAdmin.Api
{
    using AutoMapper;
    using GamesAdmin.Api.Domain.Requests;
    using GamesAdmin.Core.Models;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class DomainHandler : IRequestHandler<GetDomainRequest, DomainModel>, IRequestHandler<UpdateDomainRequest, bool>
    {
        private readonly IDomainService domainService;

        private readonly IMapper mapper;

        public DomainHandler(IDomainService domainService, IMapper mapper)
        {
            this.domainService = domainService;
            this.mapper = mapper;
        }

        public Task<bool> Handle(UpdateDomainRequest request, CancellationToken cancellationToken)
        {
            var domainModel = mapper.Map<DomainModel>(request);
            return this.domainService.CreateOrUpdate(domainModel);
        }
    

        public Task<DomainModel> Handle(GetDomainRequest request, CancellationToken cancellationToken)
          => this.domainService.Get();
    }
}
