using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetSlotBoardRequest : IRequest<bool>
    {
        public SetSlotBoardRequest(string gameConfigName, string nickname, byte[] result)
        {
            GameConfigName = gameConfigName;
            Nickname = nickname;
            Result = result;
        }

        public string GameConfigName { get; }

        public string Nickname { get; }

        public byte[] Result { get; }
    }
}