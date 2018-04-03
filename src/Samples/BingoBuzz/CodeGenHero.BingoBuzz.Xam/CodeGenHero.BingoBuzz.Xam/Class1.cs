using CodeGenHero.BingoBuzz.Xam.ModelData.BB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.BingoBuzz.Xam
{
    public static class DemoCompanies
    {
        //you can’t really use Guid.NewGuid here, because it needs to be the same, even if you regenerate.
        public static Guid DemoCompany1Id = Guid.Parse("4bbed083-7610-4aad-9945-86a6445294a0");

        public static Company DemoCompany1
        {
            get
            {
                return new Company()
                {
                    CompanyId = DemoCompany1Id,
                    Address1 = "DemoCompany1Address1",
                    Address2 = "DemoCompany1Address2",
                    City = "DemoCompany1City",
                    CodeName = "DemoCompany1CodeName",
                    Name = "DemoCompany1Name",
                    State = "DemoCompany1State",
                    WebsiteUrl = "DemoCompany1WebsiteUrl",
                    Zip = "DemoCompany1Zip",
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUsers.DemoUser1Id,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUsers.DemoUser1Id,
                    IsDeleted = false
                };
            }
        }
    }

    public static class DemoMeetings
    {
        //you can’t really use Guid.NewGuid here, because it needs to be the same, even if you regenerate.
        public static Guid DemoMeeting1Id = Guid.Parse("852c5782-8bff-410e-9ca7-c8204968c71a");

        public static Meeting DemoMeeting1
        {
            get
            {
                return new Meeting()
                {
                    Name = "MeetingName1",
                    MeetingId = DemoMeeting1Id,
                    CompanyId = DemoCompanies.DemoCompany1Id,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUsers.DemoUser1Id,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUsers.DemoUser1Id,
                    IsDeleted = false
                };
            }
        }
    }

    public static class DemoUsers
    {
        //you can’t really use Guid.NewGuid here, because it needs to be the same, even if you regenerate.
        public static Guid DemoUser1Id = Guid.Parse("94e91bfd-1c1f-4683-a617-f13e6a306165");

        public static User DemoUser1
        {
            get
            {
                return new User()
                {
                    Email = " DemoUser1Email",
                    FirstName = " SampleUser1FirstName",
                    LastName = " SampleUser1LastName",
                    UserId = DemoUser1Id,
                    CompanyId = DemoCompanies.DemoCompany1Id,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = DemoUser1Id,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = DemoUser1Id,
                    IsDeleted = false
                };
            }
        }
    }
}