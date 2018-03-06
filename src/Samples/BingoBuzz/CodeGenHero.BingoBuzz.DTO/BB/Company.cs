// <auto-generated> - Template:DTO, Version:1.0, Id:58fa7ee2-89f7-41e6-85ed-8d4482653990
namespace CodeGenHero.BingoBuzz.DTO.BB
{
	public partial class Company
	{
		public Company()
		{
			// Meetings = new System.Collections.Generic.List<Meeting>(); -- Excluded navigation property per configuration.

			InitializePartial();
		}

		public System.Guid CompanyId { get; set; } // Primary key
		public string Name { get; set; }
		public string CodeName { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string WebsiteUrl { get; set; }
		public System.DateTime CreatedDate { get; set; }
		public System.Guid CreatedUserId { get; set; }
		public System.DateTime UpdatedDate { get; set; }
		public System.Guid UpdatedUserId { get; set; }
		public bool IsDeleted { get; set; }
		// public virtual System.Collections.Generic.ICollection<Meeting> Meetings { get; set; } // Many to many mapping -- Excluded navigation property per configuration.
		public virtual User CreatedUser { get; set; } 
		public virtual User UpdatedUser { get; set; } 


		partial void InitializePartial();

	}
}