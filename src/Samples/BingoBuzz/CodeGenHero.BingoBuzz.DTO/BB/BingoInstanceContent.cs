// <auto-generated> - Template:DTO, Version:1.1, Id:58fa7ee2-89f7-41e6-85ed-8d4482653990
namespace CodeGenHero.BingoBuzz.DTO.BB
{
	public partial class BingoInstanceContent
	{
		public BingoInstanceContent()
		{
			InitializePartial();
		}

		public System.Guid BingoInstanceContentId { get; set; } // Primary key
		public System.Guid BingoContentId { get; set; }
		public System.Guid BingoInstanceId { get; set; }
		public System.Guid UserId { get; set; }
		public int Col { get; set; }
		public int Row { get; set; }
		public bool FreeSquareIndicator { get; set; }
		public int BingoInstanceContentStatusTypeId { get; set; }
		public System.DateTime CreatedDate { get; set; }
		public System.Guid CreatedUserId { get; set; }
		public System.DateTime UpdatedDate { get; set; }
		public System.Guid UpdatedUserId { get; set; }
		public bool IsDeleted { get; set; }
		public virtual System.Collections.Generic.ICollection<BingoInstanceEvent> BingoInstanceEvents { get; set; } // Many to many mapping
		public virtual BingoContent BingoContent { get; set; } 
		// public virtual BingoInstance BingoInstance { get; set; }  -- Excluded navigation property per configuration.
		public virtual BingoInstanceContentStatusType BingoInstanceContentStatusType { get; set; } 
		public virtual User CreatedUser { get; set; } 
		public virtual User UpdatedUser { get; set; } 
		public virtual User User_UserId { get; set; } 


		partial void InitializePartial();

	}
}
