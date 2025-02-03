namespace GamesAdmin.Site.Features.InternalTestAccount
{
    using MediatR;

    public class UpdateInternalTestAccountRequest : IRequest<bool>
    {
        public UpdateInternalTestAccountRequest(string[] addAccounts, string[] deletedAccounts)
        {
            AddAccounts = addAccounts;
            DeletedAccounts = deletedAccounts;
        }

        public string[] AddAccounts { get; }

        public string[] DeletedAccounts { get; }
    }
}
