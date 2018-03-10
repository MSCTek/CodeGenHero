// <auto-generated> - Template:DTO, Version:1.0, Id:58fa7ee2-89f7-41e6-85ed-8d4482653990
namespace CodeGenHero.BingoBuzz.DTO.BB
{
	public partial class RecurrenceRule
	{
		public RecurrenceRule()
		{
			// MeetingSchedules = new System.Collections.Generic.List<MeetingSchedule>(); -- Excluded navigation property per configuration.

			InitializePartial();
		}

		public System.Guid RecurrenceRuleId { get; set; } // Primary key
		public int FrequencyTypeId { get; set; }
		public System.DateTime? EndDate { get; set; }
		public int? Seconds { get; set; }
		public int? Minutes { get; set; }
		public int? Hour { get; set; }
		public int? WeekDayNum { get; set; }
		public int? OrdWeek { get; set; }
		public string WeekDay { get; set; }
		// public virtual System.Collections.Generic.ICollection<MeetingSchedule> MeetingSchedules { get; set; } // Many to many mapping -- Excluded navigation property per configuration.
		public virtual FrequencyType FrequencyType { get; set; } 


		partial void InitializePartial();

	}
}
