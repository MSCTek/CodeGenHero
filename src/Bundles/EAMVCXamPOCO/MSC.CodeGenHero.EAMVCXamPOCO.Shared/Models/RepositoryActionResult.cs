using System;
using static CodeGenHero.EAMVCXamPOCO.Enums;

namespace CodeGenHero.EAMVCXamPOCO
{
	public class RepositoryActionResult<T> where T : class
	{
		public RepositoryActionResult(T entity, RepositoryActionStatus status)
		{
			Entity = entity;
			Status = status;
		}

		public RepositoryActionResult(T entity, RepositoryActionStatus status, Exception exception) : this(entity, status)
		{
			Exception = exception;
		}

		public T Entity { get; private set; }
		public Exception Exception { get; private set; }
		public RepositoryActionStatus Status { get; private set; }
	}
}