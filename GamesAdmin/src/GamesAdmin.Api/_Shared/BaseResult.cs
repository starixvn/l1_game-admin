namespace GamesAdmin.Api._Shared
{
    public abstract class BaseResult
    {
        public string Error { get; set; }

        public bool IsSuccess => string.IsNullOrWhiteSpace(Error);
    }
}
