// <auto-generated> - Template:XamSample, Version:1.1, Id:9131a0a2-7ceb-4f4c-b8a9-6740ac19f66c
using System;
using CodeGenHero.BingoBuzz.DTO.BB;

namespace CodeGenHero.BingoBuzz.DTO.BB
{
	public static class DemoMeeting
	{
		public static System.Guid SampleMeetingId00 = Guid.Parse("2d03ace8-f067-428f-a3ab-086523f3a1a7");
		public static System.Guid SampleMeetingId01 = Guid.Parse("a3b668a6-1829-41cd-ac09-bcf88d3172de");

		public static Meeting SampleMeeting00
		{
			get
			{
				return new Meeting()
				{
					MeetingId = SampleMeetingId00,
					CompanyId = Guid.Parse("bdf0cb87-a0c0-4529-b076-23d83ca58bc7"),
					Name = "SampleName",
					CreatedDate = DateTime.Now,
					CreatedUserId = Guid.Parse("ba9aa742-9b90-46dc-b394-a93dc6b8d63d"),
					UpdatedDate = DateTime.Now,
					UpdatedUserId = Guid.Parse("5d73a35f-7316-4b19-acdc-799a82e95749"),
					IsDeleted = false,
				};
			}
		}
		public static Meeting SampleMeeting01
		{
			get
			{
				return new Meeting()
				{
					MeetingId = SampleMeetingId01,
					CompanyId = Guid.Parse("773dfe5e-00aa-429f-8020-060a12594582"),
					Name = "SampleName",
					CreatedDate = DateTime.Now,
					CreatedUserId = Guid.Parse("18054891-bf37-4117-9652-c63d4b3d33f9"),
					UpdatedDate = DateTime.Now,
					UpdatedUserId = Guid.Parse("65e17fdb-0f36-49c8-89be-5f080916ba87"),
					IsDeleted = false,
				};
			}
		}

	}
}
