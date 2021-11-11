using GamesAdmin.Core.Models.ApiLogs;

namespace GamesAdmin.Database.ApiLogs.Entities
{
    public class TicketDetailLogEntitty : ApiLogEntity
    {
        public TicketDetailRequestEntity Call { get; set; }

        public TicketDetailResultEntity Result { get; set; }
    }

    public class TicketDetailRequestEntity : SBRequestEntity
    {
        public override string RequestType => nameof(TicketDetailRequest);
    }

    public class TicketDetailResultEntity : SBResultEntity
    {
        public override string ResultType => nameof(TicketDetailResult);
    }
}