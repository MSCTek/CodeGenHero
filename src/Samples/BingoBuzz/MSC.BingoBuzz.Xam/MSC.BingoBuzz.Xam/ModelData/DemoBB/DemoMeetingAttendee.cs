using MSC.BingoBuzz.Xam.ModelData.BB;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSC.BingoBuzz.Xam.ModelData.DemoBB
{
    public static class DemoMeetingAttendee
    {
        public static MeetingAttendee SampleMeetingAttendeeAlexanderMockupReview
        {
            get
            {
                return new MeetingAttendee()
                {
                    UserId = DemoUser.UserIdAlexander,
                    MeetingId = DemoMeeting.MockupReviewMeetingId,
                    MeetingAttendeeId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false
                };
            }
        }

        public static MeetingAttendee SampleMeetingAttendeeAlexanderSprintPlanning
        {
            get
            {
                return new MeetingAttendee()
                {
                    UserId = DemoUser.UserIdAlexander,
                    MeetingId = DemoMeeting.SprintPlanningMeetingId,
                    MeetingAttendeeId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false
                };
            }
        }

        public static MeetingAttendee SampleMeetingAttendeeAlexanderSprintReview
        {
            get
            {
                return new MeetingAttendee()
                {
                    UserId = DemoUser.UserIdAlexander,
                    MeetingId = DemoMeeting.SprintReviewMeetingId,
                    MeetingAttendeeId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false
                };
            }
        }

        public static MeetingAttendee SampleMeetingAttendeeGeorgeSprintPlanning
        {
            get
            {
                return new MeetingAttendee()
                {
                    UserId = DemoUser.UserIdGeorge,
                    MeetingId = DemoMeeting.SprintPlanningMeetingId,
                    MeetingAttendeeId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false
                };
            }
        }

        public static MeetingAttendee SampleMeetingAttendeeGeorgeSprintReview
        {
            get
            {
                return new MeetingAttendee()
                {
                    UserId = DemoUser.UserIdGeorge,
                    MeetingId = DemoMeeting.SprintReviewMeetingId,
                    MeetingAttendeeId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false
                };
            }
        }

        public static MeetingAttendee SampleMeetingAttendeeThomasMockupReview
        {
            get
            {
                return new MeetingAttendee()
                {
                    UserId = DemoUser.UserIdThomas,
                    MeetingId = DemoMeeting.MockupReviewMeetingId,
                    MeetingAttendeeId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false
                };
            }
        }

        public static MeetingAttendee SampleMeetingAttendeeThomasSprintPlanning
        {
            get
            {
                return new MeetingAttendee()
                {
                    UserId = DemoUser.UserIdThomas,
                    MeetingId = DemoMeeting.SprintPlanningMeetingId,
                    MeetingAttendeeId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false
                };
            }
        }

        public static MeetingAttendee SampleMeetingAttendeeThomasSprintReview
        {
            get
            {
                return new MeetingAttendee()
                {
                    UserId = DemoUser.UserIdThomas,
                    MeetingId = DemoMeeting.SprintReviewMeetingId,
                    MeetingAttendeeId = Guid.NewGuid(),
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