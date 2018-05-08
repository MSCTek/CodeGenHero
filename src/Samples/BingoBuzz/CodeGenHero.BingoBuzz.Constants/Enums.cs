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

        public enum BingoInstanceStatusType
        {
            New = 1,
            Active = 2,
            Complete = 4
        }

        public enum BingoInstanceEventType
        {
            SquareClicked = 1,
            Bingo = 2
        }

        public enum BingoInstanceContentStatusType
        {
            Untapped = 1,
            Tapped = 2,
            Disputed = 4
        }

    }
}