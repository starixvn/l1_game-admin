using System;

namespace GamesAdmin.Api.GameServers
{
    public interface IChineseBlackjackApiFactory
    {
        IChineseBlackjackApi SetResultApi();
    }
    public class ChineseBlackjackApiFactory : IChineseBlackjackApiFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ChineseBlackjackApiFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IChineseBlackjackApi SetResultApi()
            => (IChineseBlackjackApi)serviceProvider.GetService(typeof(IChineseBlackjackApi));
    }
}
