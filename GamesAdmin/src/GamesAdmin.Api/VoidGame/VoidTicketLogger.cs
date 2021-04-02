using System;
using System.Threading.Tasks;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.VoidGame
{
    public interface IVoidTicketLogger
    {
        Task Log(DateTimeOffset requestTime, VoidTicketRequest request, DateTimeOffset responseTime, VoidGameResult result);
    }

    public class VoidTicketLogger : IVoidTicketLogger
    {
        private readonly IVoidLogRepository voidLogRepository;

        public VoidTicketLogger(IVoidLogRepository voidLogRepository)
        {
            this.voidLogRepository = voidLogRepository;
        }

        public Task Log(DateTimeOffset requestTime, VoidTicketRequest request, DateTimeOffset responseTime, VoidGameResult result)
        {
            var voidLog = new VoidLogEntity(
                requestTime,
                new VoidCall(request.ObCustId, request.SiteId, request.GameRoundId, request.GameTypeId, request.Reason, request.FreeBet),
                responseTime,
                result);

            return voidLogRepository.InsertOneAsync(voidLog);
        }
    }
}