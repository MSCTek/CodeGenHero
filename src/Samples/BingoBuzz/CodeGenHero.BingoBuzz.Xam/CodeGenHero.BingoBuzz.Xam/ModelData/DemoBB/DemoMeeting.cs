using CodeGenHero.BingoBuzz.Xam.ModelData.BB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.BingoBuzz.Xam.ModelData.DemoBB
{
    public static class DemoMeeting
    {
        public static Guid MockupReviewMeetingId = Guid.Parse("46942c60-f381-41f1-ad14-a5f373b79179");
        public static Guid SprintPlanningMeetingId = Guid.Parse("852c5782-8bff-410e-9ca7-c8204968c71a");
        public static Guid SprintReviewMeetingId = Guid.Parse("69b277bc-fc66-4c1d-80e3-554712bb70f5");

        public static Meeting SampleMeetingMockupReview
        {
            get
            {
                return new Meeting()
                {
                    Name = "BingoBuzz Mockup Review",
                    MeetingId = MockupReviewMeetingId,
                    CompanyId = DemoCompany.SampleCompanyId,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false
                };
            }
        }

        public static Meeting SampleMeetingSprintPlanning
        {
            get
            {
                return new Meeting()
                {
                    Name = "BingoBuzz Sprint Planning",
                    MeetingId = SprintPlanningMeetingId,
                    CompanyId = DemoCompany.SampleCompanyId,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false
                };
            }
        }

        public static Meeting SampleMeetingSprintReview
        {
            get
            {
                return new Meeting()
                {
                    Name = "BingoBuzz Sprint Review",
                    MeetingId = SprintReviewMeetingId,
                    CompanyId = DemoCompany.SampleCompanyId,
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