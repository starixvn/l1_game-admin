using System;

namespace GamesAdmin.Api.GameServers
{
    public interface IHiLoApiFactory
    {
        IHiLoApi SetResultApi();
    }
    public class HiLoApiFactory : IHiLoApiFactory
    {
        private readonly IServiceProvider serviceProvider;

        public HiLoApiFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IHiLoApi SetResultApi()
            => (IHiLoApi)serviceProvider.GetService(typeof(IHiLoApi));
    }
}
