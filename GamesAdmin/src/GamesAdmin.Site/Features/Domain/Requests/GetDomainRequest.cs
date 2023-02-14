namespace GamesAdmin.Site.Features.Domain
{
    using GamesAdmin.Site.Features.Domain.ViewModels;
    using MediatR;

    public class GetDomainRequest : IRequest<IndexViewModel>
    {
    }
}
