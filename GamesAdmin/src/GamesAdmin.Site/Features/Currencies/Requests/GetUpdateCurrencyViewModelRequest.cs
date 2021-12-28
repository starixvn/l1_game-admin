using GamesAdmin.Site.Features.Currencies.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Requests
{
    public class GetUpdateCurrencyViewModelRequest : IRequest<UpdateCurrencyViewModel>
    {
        public string Id { get; set; }
    }
}