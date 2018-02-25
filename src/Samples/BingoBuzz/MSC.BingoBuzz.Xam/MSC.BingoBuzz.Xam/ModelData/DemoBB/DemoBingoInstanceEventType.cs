using MSC.BingoBuzz.Xam.ModelData.BB;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSC.BingoBuzz.Xam.ModelData.DemoBB
{
    public static class DemoBingoInstanceEventType
    {
        public static BingoInstanceEventType SampleAttendeeJoinedEvent { get { return new BingoInstanceEventType() { BingoInstanceEventTypeId = 1, Name = "Attendee Joined" }; } }
        public static BingoInstanceEventType SampleBingoEvent { get { return new BingoInstanceEventType() { BingoInstanceEventTypeId = 2, Name = "Bingo" }; } }
        public static BingoInstanceEventType SampleContentDisputedEvent { get { return new BingoInstanceEventType() { BingoInstanceEventTypeId = 3, Name = "Content Disputed" }; } }
        public static BingoInstanceEventType SampleContentTappedEvent { get { return new BingoInstanceEventType() { BingoInstanceEventTypeId = 4, Name = "Content Tapped" }; } }
        public static BingoInstanceEventType SampleGameEndedEvent { get { return new BingoInstanceEventType() { BingoInstanceEventTypeId = 5, Name = "Game Ended" }; } }
        public static BingoInstanceEventType SampleGameStartedEvent { get { return new BingoInstanceEventType() { BingoInstanceEventTypeId = 6, Name = "Game Started" }; } }
    }
}