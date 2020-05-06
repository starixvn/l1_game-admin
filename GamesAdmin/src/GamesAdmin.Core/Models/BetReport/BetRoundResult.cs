using System;
using System.Collections.Generic;
using System.Text;

namespace GamesAdmin.Core.Models
{
    public class RoundResult
    {
        public long Number { get; set; }
    }

    public class BigSmallRoundResult : RoundResult
    {
        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }
    }

    public class OddEvenRoundResult : RoundResult
    {
        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }
    }
}