using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.BetLimitSuggestion.Requests;
using GamesAdmin.Site.Features.BetLimitSuggestion.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace GamesAdmin.Site.Features.BetLimitSuggestion.Handlers
{
  public class GetCurrencyGroupFilterConditionHandler : IRequestHandler<GetCurrencyGroupFilterConditionRequest, CurrencyGroupFilterConditionModel>
  {
    private readonly IConfiguration configuration;

    public GetCurrencyGroupFilterConditionHandler(IConfiguration configuration)
    {
      this.configuration = configuration;
    }

    public async Task<CurrencyGroupFilterConditionModel> Handle(GetCurrencyGroupFilterConditionRequest request, CancellationToken cancellationToken)
    {
      var model = configuration.GetSection("BetLimitSuggestion").Get<CurrencyGroupFilterConditionModel>();

      return await Task.FromResult(model);
    }

  }
}