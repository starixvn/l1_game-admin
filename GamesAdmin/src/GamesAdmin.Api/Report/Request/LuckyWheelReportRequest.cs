using System.Collections.Generic;
using GamesAdmin.Core.Models.Spins;
using MediatR;

namespace GamesAdmin.Api.Report.Request
{
    public class LuckyWheelReportRequest : IRequest<IEnumerable<SpinResult>>
    {
        public LuckyWheelReportRequest(string nickname)
        {
            Nickname = nickname;
        }

        public string Nickname { get; }
    }
}