using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class GetUpdateOperatorSettingRequest : IRequest<UpdateOperatorSettingViewModel>
    {
        public string Id { get; }

        public string OperatorId { get; }

        public string OperatorName { get; }

        public GetUpdateOperatorSettingRequest(string id, string operatorId, string operatorName)
        {
            Id = id;
            OperatorId = operatorId;
            OperatorName = operatorName;
        }
    }
}