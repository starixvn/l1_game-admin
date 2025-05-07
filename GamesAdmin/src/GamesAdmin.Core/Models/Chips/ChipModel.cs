namespace GamesAdmin.Core.Models.Chips
{
    public class ChipModel
    {
        public string Id { get; }

        public string Text { get; }

        public double Value { get; }

        public string Color { get; }

        public ChipModel(string id, string text, double value, string color)
        {
            Id = id;
            Text = text;
            Value = value;
            Color = color;
        }
    }
}