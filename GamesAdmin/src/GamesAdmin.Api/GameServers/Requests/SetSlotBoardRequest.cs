using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetSlotBoardRequest : IRequest<bool>
    {
        public SetSlotBoardRequest(string gameConfigName, string nickname, byte[] results)
        {
            GameConfigName = gameConfigName;
            Nickname = nickname;
            Results = results;
        }

        public string GameConfigName { get; }

        public string Nickname { get; }

        public byte[] Results { get; }
    }
}