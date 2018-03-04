using CodeGenHero.BingoBuzz.Xam.ModelData.BB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.BingoBuzz.Xam.ModelData.DemoBB
{
    public class DemoBingoInstanceStatusType
    {
        public static BingoInstanceStatusType SampleAbandonedStatus { get { return new BingoInstanceStatusType() { BingoInstanceStatusTypeId = 1, Name = "New" }; } }
        public static BingoInstanceStatusType SampleActiveStatus { get { return new BingoInstanceStatusType() { BingoInstanceStatusTypeId = 2, Name = "Active" }; } }
        public static BingoInstanceStatusType SampleInactiveStatus { get { return new BingoInstanceStatusType() { BingoInstanceStatusTypeId = 4, Name = "Complete" }; } }
    }
}