// <auto-generated> - Template:DTO, Version:1.0, Id:58fa7ee2-89f7-41e6-85ed-8d4482653990
namespace CodeGenHero.BingoBuzz.DTO.BB
{
	public partial class BingoInstanceStatusType
	{
		public BingoInstanceStatusType()
		{
			// BingoInstances = new System.Collections.Generic.List<BingoInstance>(); -- Excluded navigation property per configuration.

			InitializePartial();
		}

		public int BingoInstanceStatusTypeId { get; set; } // Primary key
		public string Name { get; set; }
		// public virtual System.Collections.Generic.ICollection<BingoInstance> BingoInstances { get; set; } // Many to many mapping -- Excluded navigation property per configuration.


		partial void InitializePartial();

	}
}