namespace GamesAdmin.Core.Models
{
    public class UserInfoModel
    {
        public UserInfoModel(string uid, string username, string memberName, string nickName, bool isBot)
        {
            Uid = uid;
            Username = isBot ? username : memberName;
            NickName = nickName;
            IsBot = isBot;
        }

        public string Uid { get; set; }
        public string Username { get; set; }
        public string NickName { get; set; }
        public bool IsBot { get; set; }
    }
}