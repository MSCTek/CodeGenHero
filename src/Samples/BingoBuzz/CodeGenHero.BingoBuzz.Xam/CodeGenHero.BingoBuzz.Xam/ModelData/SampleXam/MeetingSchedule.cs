// <auto-generated> - Template:XamSample, Version:1.0, Id:9131a0a2-7ceb-4f4c-b8a9-6740ac19f66c
using System;
using CodeGenHero.BingoBuzz.DTO.BB;

namespace CodeGenHero.BingoBuzz.Xam.ModelData.SampleXam
{
	public static class DemoMeetingSchedule
		{
		public static System.Guid SampleMeetingScheduleId00 = Guid.Parse("849b7e5a-d258-4ea3-a8bd-50afd721dbeb");
		public static System.Guid SampleMeetingScheduleId01 = Guid.Parse("b84f7a80-415e-4e95-bbb7-42578ccb505b");

		public static MeetingSchedule SampleMeetingSchedule00
		{
			get
			{
				return new MeetingSchedule()
				{
					MeetingScheduleId = SampleMeetingScheduleId00,
					MeetingId = Guid.Parse("9b168fdf-9a4f-4e58-ac48-f1f13037ac33"),
					StartDate = DateTime.Now,
					EndDate = DateTime.Now,
					RecurrenceRuleId = Guid.Parse("cc4ecf46-4e4e-4ed2-9966-08398a5650b2"),
					CreatedDate = DateTime.Now,
					CreatedUserId = Guid.Parse("0a5902b7-f630-4771-aa88-06b38ded51da"),
					UpdatedDate = DateTime.Now,
					UpdatedUserId = Guid.Parse("e1a596d5-3d88-4f82-a638-b9ef85ed9415"),
					IsDeleted = false,
				};
			}
		}
		public static MeetingSchedule SampleMeetingSchedule01
		{
			get
			{
				return new MeetingSchedule()
				{
					MeetingScheduleId = SampleMeetingScheduleId01,
					MeetingId = Guid.Parse("f5b4ed69-0493-41ff-ac49-9a6d13bc3c0a"),
					StartDate = DateTime.Now,
					EndDate = DateTime.Now,
					RecurrenceRuleId = Guid.Parse("a5174eba-4004-4990-b8b6-37d7346e2bc9"),
					CreatedDate = DateTime.Now,
					CreatedUserId = Guid.Parse("aa47eab9-5290-4e75-a27a-92722002ccb9"),
					UpdatedDate = DateTime.Now,
					UpdatedUserId = Guid.Parse("09e9fc5b-28b3-40f3-b5c4-c819a423f751"),
					IsDeleted = false,
				};
			}
		}

		}
	}
