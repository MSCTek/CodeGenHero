// <auto-generated> - Template:XamSample, Version:1.0, Id:9131a0a2-7ceb-4f4c-b8a9-6740ac19f66c
using System;
using CodeGenHero.BingoBuzz.DTO.BB;

namespace CodeGenHero.BingoBuzz.Xam.ModelData.SampleXam
{
	public static class DemoRecurrenceRule
		{
		public static System.Guid SampleRecurrenceRuleId00 = Guid.Parse("5232ac2d-b3bb-483e-84c6-87847c538727");
		public static System.Guid SampleRecurrenceRuleId01 = Guid.Parse("b55c38e5-2357-4b80-870e-6ece58d96203");

		public static RecurrenceRule SampleRecurrenceRule00
		{
			get
			{
				return new RecurrenceRule()
				{
					RecurrenceRuleId = SampleRecurrenceRuleId00,
					FrequencyTypeId = 0,
					EndDate = DateTime.Now,
					Seconds = 0,
					Minutes = 0,
					Hour = 0,
					WeekDayNum = 0,
					OrdWeek = 0,
					WeekDay = "SampleWeekDay",
				};
			}
		}
		public static RecurrenceRule SampleRecurrenceRule01
		{
			get
			{
				return new RecurrenceRule()
				{
					RecurrenceRuleId = SampleRecurrenceRuleId01,
					FrequencyTypeId = 0,
					EndDate = DateTime.Now,
					Seconds = 0,
					Minutes = 0,
					Hour = 0,
					WeekDayNum = 0,
					OrdWeek = 0,
					WeekDay = "SampleWeekDay",
				};
			}
		}

		}
	}
