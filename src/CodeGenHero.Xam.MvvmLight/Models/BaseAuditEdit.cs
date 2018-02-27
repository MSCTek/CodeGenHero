using GalaSoft.MvvmLight;
using System;

namespace CodeGenHero.Xam.MvvmLight
{
	public abstract class BaseAuditEdit : ObservableObject, IBaseAuditEdit
	{
		private DateTime _createdDate;
		private Guid _createdUserId;
		private bool _isDeleted;
		private DateTime _updatedDate;
		private Guid _updatedUserId;

		public DateTime CreatedDate
		{
			get { return _createdDate; }
			set { Set<DateTime>(() => CreatedDate, ref _createdDate, value); }
		}

		public Guid CreatedUserId
		{
			get { return _createdUserId; }
			set { Set<Guid>(() => CreatedUserId, ref _createdUserId, value); }
		}

		public bool IsDeleted
		{
			get { return _isDeleted; }
			set { Set<bool>(() => IsDeleted, ref _isDeleted, value); }
		}

		public DateTime UpdatedDate
		{
			get { return _updatedDate; }
			set { Set<DateTime>(() => UpdatedDate, ref _updatedDate, value); }
		}

		public Guid UpdatedUserId
		{
			get { return _updatedUserId; }
			set { Set<Guid>(() => UpdatedUserId, ref _updatedUserId, value); }
		}
	}
}