using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.CurrencyGroups.Models;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Handlers
{
  public class CloneCurrencyGroupHandler : IRequestHandler<CloneCurrencyGroupRequest, CurrencyGroupModel>
  {
    private readonly ICurrencyGroupApi api;
    private readonly IMapper mapper;

    public CloneCurrencyGroupHandler(ICurrencyGroupApi api, IMapper mapper)
    {
      this.api = api;
      this.mapper = mapper;
    }

    public async Task<CurrencyGroupModel> Handle(CloneCurrencyGroupRequest request, CancellationToken cancellationToken)
        => await api.Clone(mapper.Map<CloneCurrencyGroupModel>(request.Model));
  }
}
