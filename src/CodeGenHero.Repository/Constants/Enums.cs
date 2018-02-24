using System;

namespace CodeGenHero.Repository
{
	public static class Enums
	{
		[Flags]
		public enum RepositoryActionStatus
		{
			Ok = 1,
			Created = 2,
			Updated = 4,
			NotFound = 8,
			Deleted = 16,
			NothingModified = 32,
			Error = 64
		}
	}
}