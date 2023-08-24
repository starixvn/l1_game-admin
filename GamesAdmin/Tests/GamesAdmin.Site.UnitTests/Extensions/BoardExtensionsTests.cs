using GamesAdmin.Site.Extensions;
using System.Collections.Generic;
using Xunit;

namespace GamesAdmin.Site.UnitTests.Extensions
{
    public class BoardExtensionsTests
    {
        [Fact]
        public void FormatBoard_ExistBoard_ReturnExpected()
        {
            // arrange
            var board = new List<byte>
            {
                3,0,2,6,6,2,2,1,4,4,4,1,5,2,9,5,5,3,1,1,0,3,7,4,3,8,2,2,4,4
            };
            var expected = new byte[5][];
            expected[0] = new byte[6] { 3, 2, 4, 5, 0, 8 };
            expected[1] = new byte[6] { 0, 2, 1, 5, 3, 2 };
            expected[2] = new byte[6] { 2, 1, 5, 3, 7, 2 };
            expected[3] = new byte[6] { 6, 4, 2, 1, 4, 4 };
            expected[4] = new byte[6] { 6, 4, 9, 1, 3, 4 };

            // act
            var actual = BoardExtensions.FormatBoard(board.ToArray(), 5, 6);

            // assert

            Assert.Equal(expected, actual);
        }
    }
}
