using System;

namespace CodeGenHero.BingoBuzz.Constants
{
    public static class Enums
    {
        public enum QueueableObjects
        {
            BingoInstance,
            BingoInstanceEvent,
        }

        public enum BingoInstanceEventType
        {
            SquareClicked = 1,
            Bingo = 2,
        }

    }
}