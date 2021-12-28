using GamesAdmin.Core.Models.ApiLogs;

namespace GamesAdmin.Database.ApiLogs.Entities
{
    public class TicketDetailLogEntitty : ApiLogEntity
    {
        public TicketDetailRequestEntity Call { get; set; }

        public TicketDetailResultEntity Result { get; set; }
    }

    public class TicketDetailRequestEntity : ApiLogRequestEntity
    {
        public override string RequestType => nameof(TicketDetailRequest);

        public string Language { get; set; }

        public bool FreeBet { get; set; }

        public Auth Auth { get; set; }
    }

    public class TicketDetailResultEntity : ApiLogResultEntity
    {
        public override string ResultType => nameof(TicketDetailResult);

        public string RedirectUrl { get; set; }

        public string Error { get; set; }
    }
}