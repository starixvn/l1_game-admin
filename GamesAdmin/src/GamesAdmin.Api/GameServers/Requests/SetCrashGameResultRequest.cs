using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetCrashGameResultRequest : IRequest<bool>
    {
        public SetCrashGameResultRequest(string gameConfigName, float result)
        {
            this.gameConfigName = gameConfigName;
            this.result = result;
        }

        public string gameConfigName { get; set; }

        public float result { get; set; }
    }
}
