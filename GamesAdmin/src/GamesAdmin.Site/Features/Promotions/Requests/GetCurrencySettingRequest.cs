using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class GetCurrencySettingRequest : IRequest<EditCurrencyGroupSettingViewModel>
    {
        public GetCurrencySettingRequest(string id, string operatorId, string groupId)
        {
            Id = id;
            OperatorId = operatorId;
            GroupId = groupId;
        }

        public string GroupId { get;  }

        public string OperatorId { get; }

        public string Id { get; }
    }
}
