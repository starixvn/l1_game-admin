namespace GamesAdmin.Core.Models.Chips
{
    public class ChipModel
    {
        public string Id { get; }

        public string Text { get; }

        public int Value { get; }

        public string Color { get; }

        public ChipModel(string id, string text, int value, string color)
        {
            Id = id;
            Text = text;
            Value = value;
            Color = color;
        }
    }
}