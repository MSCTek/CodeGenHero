using CodeGenHero.BingoBuzz.Xam.ModelData.BB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.BingoBuzz.Xam.ModelData.DemoBB
{
    public static class DemoBingoInstance
    {
        public static Guid SampleBingoInstanceId01 = Guid.Parse("fdd61ced-d869-45e2-b843-6b7b6d6ce14b");
        public static Guid SampleBingoInstanceId02 = Guid.Parse("03733ac4-43a1-4341-834a-6bd40e2a7296");
        public static Guid SampleBingoInstanceId03 = Guid.Parse("6ed0ac94-93c4-4bb1-9fe1-90ce5aa9b99b");

        public static BingoInstance SampleBingoInstanceMockupReview
        {
            get
            {
                return new BingoInstance()
                {
                    NumberOfColumns = 5,
                    NumberOfRows = 5,
                    IncludeFreeSquareIndicator = true,
                    MeetingId = DemoMeeting.MockupReviewMeetingId,
                    BingoInstanceId = SampleBingoInstanceId01,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false,
                    BingoInstanceStatusTypeId = DemoBingoInstanceStatusType.SampleInactiveStatus.BingoInstanceStatusTypeId
                };
            }
        }

        public static BingoInstance SampleBingoInstanceSprintPlanning
        {
            get
            {
                return new BingoInstance()
                {
                    NumberOfColumns = 5,
                    NumberOfRows = 5,
                    IncludeFreeSquareIndicator = true,
                    MeetingId = DemoMeeting.SprintPlanningMeetingId,
                    BingoInstanceId = SampleBingoInstanceId02,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false,
                    BingoInstanceStatusTypeId = DemoBingoInstanceStatusType.SampleInactiveStatus.BingoInstanceStatusTypeId
                };
            }
        }

        public static BingoInstance SampleBingoInstanceSprintReview
        {
            get
            {
                return new BingoInstance()
                {
                    NumberOfColumns = 5,
                    NumberOfRows = 5,
                    IncludeFreeSquareIndicator = true,
                    MeetingId = DemoMeeting.SprintReviewMeetingId,
                    BingoInstanceId = SampleBingoInstanceId03,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser.UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser.UserIdAlexander,
                    IsDeleted = false,
                    BingoInstanceStatusTypeId = DemoBingoInstanceStatusType.SampleInactiveStatus.BingoInstanceStatusTypeId
                };
            }
        }
    }
}