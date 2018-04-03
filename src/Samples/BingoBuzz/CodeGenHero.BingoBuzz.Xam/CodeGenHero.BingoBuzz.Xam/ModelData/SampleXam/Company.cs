// <auto-generated> - Template:XamSample, Version:1.0, Id:9131a0a2-7ceb-4f4c-b8a9-6740ac19f66c
using System;
using CodeGenHero.BingoBuzz.DTO.BB;

namespace CodeGenHero.BingoBuzz.Xam.ModelData.SampleXam
{
	public static class DemoCompany
		{
		public static System.Guid SampleCompanyId00 = Guid.Parse("508f544a-35bc-4636-8274-41e4b1e2395c");
		public static System.Guid SampleCompanyId01 = Guid.Parse("a4733bf1-8f7e-4e9b-9bd4-f484172d1c13");

		public static Company SampleCompany00
		{
			get
			{
				return new Company()
				{
					CompanyId = SampleCompanyId00,
					Name = "SampleName",
					CodeName = "SampleCodeName",
					Address1 = "SampleAddress1",
					Address2 = "SampleAddress2",
					City = "SampleCity",
					State = "SampleState",
					Zip = "SampleZip",
					WebsiteUrl = "SampleWebsiteUrl",
					CreatedDate = DateTime.Now,
					CreatedUserId = Guid.Parse("6a6e6878-0c46-48f9-8acd-6553c9d77d56"),
					UpdatedDate = DateTime.Now,
					UpdatedUserId = Guid.Parse("1bcc572e-f02f-42e8-9bae-01d3c73a1598"),
					IsDeleted = false,
				};
			}
		}
		public static Company SampleCompany01
		{
			get
			{
				return new Company()
				{
					CompanyId = SampleCompanyId01,
					Name = "SampleName",
					CodeName = "SampleCodeName",
					Address1 = "SampleAddress1",
					Address2 = "SampleAddress2",
					City = "SampleCity",
					State = "SampleState",
					Zip = "SampleZip",
					WebsiteUrl = "SampleWebsiteUrl",
					CreatedDate = DateTime.Now,
					CreatedUserId = Guid.Parse("6e373c88-2701-44a4-9b58-948554c7021f"),
					UpdatedDate = DateTime.Now,
					UpdatedUserId = Guid.Parse("2220384b-70f2-4da9-a622-5bd8634ea24a"),
					IsDeleted = false,
				};
			}
		}

		}
	}
