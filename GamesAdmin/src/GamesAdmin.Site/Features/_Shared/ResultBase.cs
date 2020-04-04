namespace GamesAdmin.Site.Features._Shared
{
    public class ResultBase
    {
        public ResultBase(string error) 
        {
            Error = error;
        }

        public string Error { get; }

        public bool Success => string.IsNullOrWhiteSpace(Error);
    }
}
