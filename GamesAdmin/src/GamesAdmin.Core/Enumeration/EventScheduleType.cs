namespace GamesAdmin.Core.Enumeration
{
    public class EventScheduleType : Enumeration
    {
        public const string UnderMaintenanceValue = "0";
        public const string PromotionValue = "1";

        public static readonly EventScheduleType UnderMaintenance = new EventScheduleType(UnderMaintenanceValue, "Under Maintenance");
        public static readonly EventScheduleType Promotion = new EventScheduleType(PromotionValue, "Promotion");


        public EventScheduleType()
        {
        }

        public EventScheduleType(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
