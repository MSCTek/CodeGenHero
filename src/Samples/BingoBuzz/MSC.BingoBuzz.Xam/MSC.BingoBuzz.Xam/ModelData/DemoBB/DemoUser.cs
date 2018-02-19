using MSC.BingoBuzz.Xam.ModelData.BB;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSC.BingoBuzz.Xam.ModelData.DemoBB
{
    public static class DemoUser
    {
        public static Guid UserIdAlexander = Guid.Parse("94e91bfd-1c1f-4683-a617-f13e6a306165");
        public static Guid UserIdGeorge = Guid.Parse("2dc63540-ae6e-4b96-b01e-fffd980de709");
        public static Guid UserIdThomas = Guid.Parse("d859fb8f-3d13-48ad-971d-154e41ac9f51");

        public static User UserAlexander
        {
            get
            {
                return new User()
                {
                    Email = "alexander@msctek.com",
                    FirstName = "Alexander",
                    LastName = "Hamilton",
                    UserId = UserIdAlexander,
                    CompanyId = DemoCompany.SampleCompanyId,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = UserIdAlexander,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = UserIdAlexander,
                    IsDeleted = false
                };
            }
        }

        public static User UserGeorge
        {
            get
            {
                return new User()
                {
                    Email = "george@msctek.com",
                    FirstName = "George",
                    LastName = "Washington",
                    UserId = UserIdGeorge,
                    CompanyId = DemoCompany.SampleCompanyId,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = UserIdGeorge,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = UserIdGeorge,
                    IsDeleted = false
                };
            }
        }

        public static User UserThomas
        {
            get
            {
                return new User()
                {
                    Email = "thomas@msctek.com",
                    FirstName = "Thomas",
                    LastName = "Jefferson",
                    UserId = UserIdThomas,
                    CompanyId = DemoCompany.SampleCompanyId,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = UserIdThomas,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserId = UserIdThomas,
                    IsDeleted = false
                };
            }
        }
    }
}