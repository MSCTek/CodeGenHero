// <auto-generated> - Template:SqliteModelData, Version:1.0, Id:c5caad15-b3be-4443-87d8-7cabde59f7b0
using CodeGenHero.Xam.Sqlite;
using SQLite;

namespace CodeGenHero.BingoBuzz.Xam.ModelData.BB
{
	[Table("Company")]
	public partial class Company : BaseAuditEdit
	{
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string CodeName { get; set; }

		[PrimaryKey]
		public System.Guid CompanyId { get; set; }

		public string Name { get; set; }
		public string State { get; set; }
		public string WebsiteUrl { get; set; }
		public string Zip { get; set; }
	}
}