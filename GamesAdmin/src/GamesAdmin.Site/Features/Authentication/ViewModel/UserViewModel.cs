namespace GamesAdmin.Site.Features.Authentication.ViewModel
{
    public class UserViewModel
    {
        public UserViewModel(bool isLoginSuccessfully, string jwtToken = "")
        {
            IsLoginSuccessfully = isLoginSuccessfully;
            JwtToken = jwtToken;
        }

        public bool IsLoginSuccessfully { get; }

        public string JwtToken { get; }
    }
}
