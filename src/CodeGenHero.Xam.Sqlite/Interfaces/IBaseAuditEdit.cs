using System;

namespace CodeGenHero.Xam.Sqlite
{
	public interface IBaseAuditEdit
	{
		DateTime CreatedDate { get; set; }
		Guid CreatedUserId { get; set; }
		bool IsDeleted { get; set; }
		DateTime UpdatedDate { get; set; }
		Guid UpdatedUserId { get; set; }
	}
}