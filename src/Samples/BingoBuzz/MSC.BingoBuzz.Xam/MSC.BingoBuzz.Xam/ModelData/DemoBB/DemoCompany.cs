using MSC.BingoBuzz.Xam.ModelData.BB;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSC.BingoBuzz.Xam.ModelData.DemoBB
{
    public static class DemoCompany
    {
        public static Guid SampleCompanyId = Guid.Parse("6b0c428b-20bd-47c1-9d91-040e5a377fff");

        public static Company SampleCompanyUSA
        {
            get
            {
                return new Company()
                {
                    Name = "United States of America",
                    Address1 = "123 Main Street",
                    Address2 = "Suite 123",
                    City = "AnyCity",
                    CodeName = "USA",
                    CompanyId = SampleCompanyId,
                    State = "AnyState",
                    WebsiteUrl = "http://www.website.com",
                    Zip = "12345",
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