namespace GamesAdmin.Site.Features.Domain
{
    using ViewModels;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using GamesAdmin.Core.Models;

    public class DomainHandler : IRequestHandler<GetDomainRequest, IndexViewModel>, IRequestHandler<UpdateDomainRequest, bool>
    {
        private readonly IDomainService domainService;

        private readonly IMapper mapper;

        public DomainHandler(IDomainService domainService, IMapper mapper)
        {
            this.domainService = domainService;
            this.mapper = mapper;
        }

        public async Task<IndexViewModel> Handle(GetDomainRequest request, CancellationToken cancellationToken)
        {
            var domainModel = await domainService.Get();
            return new IndexViewModel
            {
                DomainId = domainModel.Id,
                Value = domainModel.Value
            };
        }
  

        public Task<bool> Handle(UpdateDomainRequest request, CancellationToken cancellationToken)
        {
            var domainModel = mapper.Map<DomainModel>(request);
            return domainService.CreateOrUpdate(domainModel);
        }
  
    }
}
