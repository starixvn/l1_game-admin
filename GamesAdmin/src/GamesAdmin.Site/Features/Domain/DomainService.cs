namespace GamesAdmin.Site.Features.Domain
{
    using GamesAdmin.Core.Models;
    using System.Threading.Tasks;

    public interface IDomainService
    {
        Task<DomainModel> Get();

        Task<bool> CreateOrUpdate(DomainModel model);
    }

    public class DomainService : IDomainService
    {
        private readonly IDomainApi domainApi;

        public DomainService(IDomainApi domainApi)
        {
            this.domainApi = domainApi;
        }

        public Task<bool> CreateOrUpdate(DomainModel model)
           => domainApi.Update(model);

        public Task<DomainModel> Get()
           => domainApi.Get();
    }
}
