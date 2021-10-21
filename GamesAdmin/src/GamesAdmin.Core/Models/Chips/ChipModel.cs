namespace GamesAdmin.Core.Models.Chips
{
    public class ChipModel
    {
        public string Id { get; }

        public string Text { get; }

        public int Value { get; }

        public ChipModel(string id, string text, int value)
        {
            Id = id;
            Text = text;
            Value = value;
        }
    }
}