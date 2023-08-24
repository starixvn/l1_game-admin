namespace GamesAdmin.Site.UnitTests.Report.ViewModels
{
    using GamesAdmin.Core.Models.GameConfigs;
    using GamesAdmin.Site.Features._Shared.Helpers;
    using GamesAdmin.Site.Features.Report.ViewModels;
    using Xunit;

    public class JackpotRecordViewModelTests
    {
        [Theory]
        [InlineData(5000.00, 1000, "5,000.00")]
        [InlineData(5000.00, 699.888, "7,144.00")]
        [InlineData(5000.00, 17.8228, "280,539.53")]
        [InlineData(5000.00, 15587.756, "320.76")]
        [InlineData(5000.00, 25363.225, "197.14")]
        [InlineData(5000.00, 1575.7, "3,173.19")]
        [InlineData(5000.00, 2996.5779, "1,668.57")]
        [InlineData(5000.00, 169.4249, "29,511.60")]
        [InlineData(5000.00, 2153.8875, "2,321.38")]
        [InlineData(5000.00, 5102.4731, "979.92")]
        [InlineData(5000.00, 26074.8389, "191.76")]
        [InlineData(5000.00, 1, "5,000,000.00")]

        [InlineData(25000.00, 1000, "25,000.00")]
        [InlineData(25000.00, 699.888, "35,720.00")]
        [InlineData(25000.00, 17.8228, "1,402,697.67")]
        [InlineData(25000.00, 15587.756, "1,603.82")]
        [InlineData(25000.00, 25363.225, "985.68")]
        [InlineData(25000.00, 1575.7, "15,865.96")]
        [InlineData(25000.00, 2996.5779, "8,342.85")]
        [InlineData(25000.00, 169.4249, "147,558.00")]
        [InlineData(25000.00, 2153.8875, "11,606.92")]
        [InlineData(25000.00, 5102.4731, "4,899.58")]
        [InlineData(25000.00, 26074.8389, "958.78")]
        [InlineData(25000.00, 1, "25,000,000.00")]

        [InlineData(50000.00, 1000, "50,000.00")]
        [InlineData(50000.00, 699.888, "71,440.00")]
        [InlineData(50000.00, 17.8228, "2,805,395.34")]
        [InlineData(50000.00, 15587.756, "3,207.65")]
        [InlineData(50000.00, 25363.225, "1,971.36")]
        [InlineData(50000.00, 1575.7, "31,731.93")]
        [InlineData(50000.00, 2996.5779, "16,685.70")]
        [InlineData(50000.00, 169.4249, "295,116.01")]
        [InlineData(50000.00, 2153.8875, "23,213.84")]
        [InlineData(50000.00, 5102.4731, "9,799.17")]
        [InlineData(50000.00, 26074.8389, "1,917.56")]
        [InlineData(50000.00, 1, "50,000,000.00")]

        [InlineData(125000.00, 1000, "125,000.00")]
        [InlineData(125000.00, 699.888, "178,600.00")]
        [InlineData(125000.00, 17.8228, "7,013,488.34")]
        [InlineData(125000.00, 15587.756, "8,019.11")]
        [InlineData(125000.00, 25363.225, "4,928.40")]
        [InlineData(125000.00, 1575.7, "79,329.82")]
        [InlineData(125000.00, 2996.5779, "41,714.25")]
        [InlineData(125000.00, 169.4249, "737,790.02")]
        [InlineData(125000.00, 2153.8875, "58,034.60")]
        [InlineData(125000.00, 5102.4731, "24,497.92")]
        [InlineData(125000.00, 26074.8389, "4,793.89")]
        [InlineData(125000.00, 1, "125,000,000.00")]

        [InlineData(1350000.00, 1000, "1,350,000.00")]
        [InlineData(1350000.00, 699.888, "1,928,880.05")]
        [InlineData(1350000.00, 17.8228, "75,745,674.08")]
        [InlineData(1350000.00, 15587.756, "86,606.44")]
        [InlineData(1350000.00, 25363.225, "53,226.67")]
        [InlineData(1350000.00, 1575.7, "856,762.07")]
        [InlineData(1350000.00, 2996.5779, "450,513.90")]
        [InlineData(1350000.00, 169.4249, "7,968,132.19")]
        [InlineData(1350000.00, 2153.8875, "626,773.68")]
        [InlineData(1350000.00, 5102.4731, "264,577.58")]
        [InlineData(1350000.00, 26074.8389, "51,774.05")]
        [InlineData(1350000.00, 1, "1,350,000,000.00")]

        [InlineData(453688.87, 1000, "453,688.87")]
        [InlineData(453688.87, 699.888, "648,230.67")]
        [InlineData(453688.87, 17.8228, "25,455,532.80")]
        [InlineData(453688.87, 15587.756, "29,105.46")]
        [InlineData(453688.87, 25363.225, "17,887.66")]
        [InlineData(453688.87, 1575.7, "287,928.46")]
        [InlineData(453688.87, 2996.5779, "151,402.33")]
        [InlineData(453688.87, 169.4249, "2,677,816.96")]
        [InlineData(453688.87, 2153.8875, "210,637.22")]
        [InlineData(453688.87, 5102.4731, "88,915.48")]
        [InlineData(453688.87, 26074.8389, "17,399.49")]
        [InlineData(453688.87, 1, "453,688,870.00")]

        [InlineData(50179.00, 1000, "50,179.00")]
        [InlineData(50179.00, 699.888, "71,695.76")]
        [InlineData(50179.00, 17.8228, "2,815,438.65")]
        [InlineData(50179.00, 15587.756, "3,219.13")]
        [InlineData(50179.00, 25363.225, "1,978.42")]
        [InlineData(50179.00, 1575.7, "31,845.53")]
        [InlineData(50179.00, 2996.5779, "16,745.43")]
        [InlineData(50179.00, 169.4249, "296,172.52")]
        [InlineData(50179.00, 2153.8875, "23,296.95")]
        [InlineData(50179.00, 5102.4731, "9,834.25")]
        [InlineData(50179.00, 26074.8389, "1,924.42")]
        [InlineData(50179.00, 1, "50,179,000.00")]
        public void ConvertToUserCurrency_WithINHBase_ReturnExpected(decimal amount, decimal rate, string expected)
        {
            // arrange
            var baseStakeLevel = new StakeLevel[] {
                    new StakeLevel(0.1, 0, 0.4),
                    new StakeLevel(0.2, 0, 0.4),
                    new StakeLevel(1, 0, 0.2)
            };
            var stakeLevelsByCurrency = new StakeLevel[] {
                    new StakeLevel(0.2, 0, 0.5),
                    new StakeLevel(0.3, 0, 0.6),
                    new StakeLevel(1, 0, 0.2)
            };
            var viewModel = new JackpotRecordViewModel()
            {
                WinningAmount = amount,
                Rate = rate,
                BaseStakeLevel = baseStakeLevel,
                StakeLevelByCurrency = stakeLevelsByCurrency
            };

            // act
            viewModel.ConvertToUserCurrency(true, 1000);

            // assert
            Assert.Equal(HtmlHelpers.formatNumber(viewModel.WinningAmount), expected);
        }
    }
}
