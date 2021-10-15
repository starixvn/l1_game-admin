using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetAllHandler : Handler, IRequestHandler<GetAllRequest, CurrencyManagementViewModel>
    {
        public GetAllHandler(IMapper mapper, ICurrencyApi api) : base(mapper, api)
        {
        }

        public async Task<CurrencyManagementViewModel> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            var models = await Api.GetAll();

            return new CurrencyManagementViewModel
            {
                Models = models
            };
        }
    }
}