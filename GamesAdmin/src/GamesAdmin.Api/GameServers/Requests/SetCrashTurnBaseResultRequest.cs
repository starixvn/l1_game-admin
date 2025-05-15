using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetCrashTurnBaseResultRequest : IRequest<bool>
    {
        public SetCrashTurnBaseResultRequest(string gameConfigName, float result, string nickName)
        {
            this.gameConfigName = gameConfigName;
            this.result = result;
            this.nickName = nickName;
        }

        public string gameConfigName { get; set; }

        public float result { get; set; }

        public string nickName { get; set; }
    }
}
