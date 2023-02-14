namespace GamesAdmin.Api.Domain.Requests
{
    using GamesAdmin.Core.Models;
    using MediatR;

    public class UpdateDomainRequest : IRequest<bool>
    {
        public UpdateDomainRequest(DomainModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public string Value { get; }

        public string Id { get; }
    }
}
