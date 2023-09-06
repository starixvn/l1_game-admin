using System;

namespace GamesAdmin.Api.GameServers
{
    public interface IPontoonApiFactory
    {
        IPontoonApi SetResultApi();
    }
    public class PontoonApiFactory : IPontoonApiFactory
    {
        private readonly IServiceProvider serviceProvider;

        public PontoonApiFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IPontoonApi SetResultApi()
            => (IPontoonApi)serviceProvider.GetService(typeof(IPontoonApi));
    }
}
