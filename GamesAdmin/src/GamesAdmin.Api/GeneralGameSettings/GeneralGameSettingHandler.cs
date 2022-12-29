using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GeneralGameSettings.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GeneralGameSettings
{
    public class GeneralGameSettingHandler :
        IRequestHandler<GetAllRequest, IEnumerable<GeneralGameSetting>>
    {
        private readonly IGeneralGameSettingService generalGameSettingService;

        public GeneralGameSettingHandler(IGeneralGameSettingService generalGameSettingService)
        {
            this.generalGameSettingService = generalGameSettingService;
        }

        public Task<IEnumerable<GeneralGameSetting>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        => generalGameSettingService.GetAll();
    }
}