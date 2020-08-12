namespace GamesAdmin.Api._Shared
{
    public abstract class BaseResult
    {
        public string ErrorCode { get; set; } = "0";

        public string ErrorDescription { get; set; }

        public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorDescription);
    }
}
