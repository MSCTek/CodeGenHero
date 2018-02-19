using CodeGenHero.EAMVCXamPOCO.Xam.ModelObj;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSC.BingoBuzz.Xam.ModelObj.BB
{
    public partial class MeetingSchedule : BaseAuditEdit
    {
        public string StartDateDisplay => StartDate.HasValue ? ((DateTime)StartDate).ToString("MMM d, yyyy  h:mm tt") : string.Empty;
    }
}