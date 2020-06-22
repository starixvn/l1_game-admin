namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoResultViewModel
    {
        public KenoResultViewModel(byte[] results)
        {
            Results = results;
        }

        public byte[] Results { get; }

        public override string ToString()
        {
            return string.Join("; ", Results);
        }
    }
}
