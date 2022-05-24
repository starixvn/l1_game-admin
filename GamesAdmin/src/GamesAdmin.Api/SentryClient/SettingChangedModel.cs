namespace GamesAdmin.Api.SentryClient
{
    public class SettingChangedModel
    {
        public SettingChangedModel() { }

        public SettingChangedModel(string oldValue, string newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public SettingChangedModel(string name, string oldValue, string newValue)
        {
            Name = name;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public string Name { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
