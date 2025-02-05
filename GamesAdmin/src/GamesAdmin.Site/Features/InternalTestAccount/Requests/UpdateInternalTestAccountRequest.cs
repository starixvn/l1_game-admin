namespace GamesAdmin.Site.Features.InternalTestAccount
{
    using GamesAdmin.Core.Models;
    using MediatR;

    public class UpdateInternalTestAccountRequest : IRequest<UpdateInternalUUSTestResult>
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
