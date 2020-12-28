namespace GamesAdmin.Site.Extensions
{
    public static class BoardExtensions
    {
        public static byte[][] FormatBoard(this byte[] symbols, int numberOfRows = 3, int numberOfCols = 5)
        {
            byte[][] formatResult = new byte[numberOfRows][];

            for (var row = 0; row < numberOfRows; row++)
            {
                formatResult[row] = new byte[numberOfCols];
                for (var column = 0; column < numberOfCols; column++)
                {
                    formatResult[row][column] = symbols[row + column * numberOfRows];
                }
            }

            return formatResult;
        }
    }
}