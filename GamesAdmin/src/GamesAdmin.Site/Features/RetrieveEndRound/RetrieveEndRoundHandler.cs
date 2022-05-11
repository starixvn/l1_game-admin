namespace GamesAdmin.Site.Features.RetrieveEndRound
{
    using System.Threading;
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.Partners;
    using GamesAdmin.Site.Features.RetrieveEndRound.Request;
    using MediatR;

    public class RetrieveEndRoundHandler : IRequestHandler<GetRetrieveEndRoundRequest, string>
    {
        private readonly IPartnerService operatorService;
        private readonly IRetrieveEndRoundService retrieveEndRoundService;

        public RetrieveEndRoundHandler(IPartnerService operatorService, IRetrieveEndRoundService retrieveEndRoundService)
        {
            this.operatorService = operatorService;
            this.retrieveEndRoundService = retrieveEndRoundService;
        }

        public async Task<string> Handle(GetRetrieveEndRoundRequest request, CancellationToken cancellationToken)
        {
            var Operator = await this.operatorService.GetById(request.Model.OperatorId);
            if (Operator != null)
            {
                request.Model.OperatorSecret = Operator.SecretKey;
            }
            return await retrieveEndRoundService.GetRetrieveEndRoundAsync(request.Model);
        }
    }
}
