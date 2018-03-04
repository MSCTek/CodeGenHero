using CodeGenHero.Xam.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.BingoBuzz.Xam.ModelObj.BB
{
    public partial class MeetingSchedule : BaseAuditEdit
    {
        public string StartDateDisplay => StartDate.HasValue ? ((DateTime)StartDate).ToString("MMM d, yyyy  h:mm tt") : string.Empty;
    }
}