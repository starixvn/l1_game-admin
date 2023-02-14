namespace GamesAdmin.Site.Features.Domain
{
    using MediatR;

    public class UpdateDomainRequest : IRequest<bool>
    {
        public UpdateDomainRequest(string id, string value)
        {
            Value = value;
            Id = id;
        }

        public string Value { get; }

        public string Id { get; }
    }
}
