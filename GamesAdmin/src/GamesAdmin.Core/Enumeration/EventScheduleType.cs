namespace GamesAdmin.Core.Enumeration
{
    public class EventScheduleType : Enumeration
    {
        public const string UnderMaintenanceValue = "1";
        public const string PromotionValue = "2";
        public const string NotificationValue = "3";

        public static readonly EventScheduleType UnderMaintenance = new EventScheduleType(UnderMaintenanceValue, "Under Maintenance");
        public static readonly EventScheduleType Promotion = new EventScheduleType(PromotionValue, "Promotion");
        public static readonly EventScheduleType Notification = new EventScheduleType(NotificationValue, "Notification");


        public EventScheduleType()
        {
        }

        public EventScheduleType(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
