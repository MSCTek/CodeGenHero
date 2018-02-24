using CodeGenHero.EAMVCXamPOCO.Xam.ModelData;
using MSC.BingoBuzz.Xam.ModelData.BB;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSC.BingoBuzz.Xam.ModelData.DemoBB
{
    public static class DemoMeetingSchedule
    {
        public static MeetingSchedule SampleMeetingSchedule3DaysAway9am
        {
            get
            {
                var now = DateTime.Now;
                var date = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0);
                date.AddDays(3);
                date.ToUniversalTime();
                return new MeetingSchedule()
                {
                    StartDate = date,
                    EndDate = date.AddHours(2),
                    MeetingId = DemoMeeting.MockupReviewMeetingId,
                    MeetingScheduleId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false
                };
            }
        }

        public static MeetingSchedule SampleMeetingSchedule5DaysAway11am
        {
            get
            {
                var now = DateTime.Now;
                var date = new DateTime(now.Year, now.Month, now.Day, 11, 0, 0);
                date.AddDays(5);
                date.ToUniversalTime();
                return new MeetingSchedule()
                {
                    StartDate = date,
                    EndDate = date.AddHours(1),
                    MeetingId = DemoMeeting.SprintReviewMeetingId,
                    MeetingScheduleId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false
                };
            }
        }

        public static MeetingSchedule SampleMeetingSchedule5DaysAway1pm
        {
            get
            {
                var now = DateTime.Now;
                var date = new DateTime(now.Year, now.Month, now.Day, 13, 0, 0);
                date.AddDays(5);
                date.ToUniversalTime();
                return new MeetingSchedule()
                {
                    StartDate = date,
                    EndDate = date.AddHours(13),
                    MeetingId = DemoMeeting.SprintPlanningMeetingId,
                    MeetingScheduleId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false
                };
            }
        }
    }
}