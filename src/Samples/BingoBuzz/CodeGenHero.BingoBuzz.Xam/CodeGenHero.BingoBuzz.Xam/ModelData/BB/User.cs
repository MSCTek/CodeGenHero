// <auto-generated> - Template:SqliteModelData, Version:1.0, Id:c5caad15-b3be-4443-87d8-7cabde59f7b0
using CodeGenHero.Xam.Sqlite;
using SQLite;

namespace CodeGenHero.BingoBuzz.Xam.ModelData.BB
{
	[Table("User")]
	public partial class User : BaseAuditEdit
	{
		public System.Guid CompanyId { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		[PrimaryKey]
		public System.Guid UserId { get; set; }

	}
}