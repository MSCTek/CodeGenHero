using System;

namespace CodeGenHero.Xam.Sqlite
{
	public abstract class BaseAuditEdit : IBaseAuditEdit
	{
		public DateTime CreatedDate { get; set; }

		public Guid CreatedUserId { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime UpdatedDate { get; set; }

		public Guid UpdatedUserId { get; set; }
	}
}