using System.Collections.Generic;
using GamesAdmin.Core.Models.SlotGame;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetCascadingSlotResultRequest : IRequest<IEnumerable<SettlementCascadingSlotResult>>
    {
        public string gameConfigName { get; set; }

        public string name { get; set; }

        public CascadingSlotResultRequest result { get; set; }

        public bool IsValid
        {
            get
            {
                return result.@base.board.Count == 30;
            }
        }
    }

    public class CascadingSlotResultRequest
    {
        public BaseSpinRequest @base { get; set; }

        public List<TumbleRequest> tumbles { get; set; }
    }

    public class BaseSpinRequest
    {
        public List<int> board { get; set; }

        public List<int> multipliers { get; set; }
    }

    public class TumbleRequest
    {
        public List<int> newSymbols { get; set; }

        public List<int> multipliers { get; set; }
    }
}