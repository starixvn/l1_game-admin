namespace GamesAdmin.Site.Features._Shared
{
    public class MessageResult
    {
        public MessageResult(string message)
        {
            Success = string.IsNullOrWhiteSpace(message);
            Message = message;
        }

        public MessageResult(bool success, string messge)
        {
            Success = success;
            Message = messge;
        }

        public string Message { get; }

        public bool Success { get; }
    }

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