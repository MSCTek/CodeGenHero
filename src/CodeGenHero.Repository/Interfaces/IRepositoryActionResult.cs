using System;

namespace CodeGenHero.Repository
{
	public interface IRepositoryActionResult<T> where T : class
	{
		T Entity { get; }
		Exception Exception { get; }
		Enums.RepositoryActionStatus Status { get; }
	}
}