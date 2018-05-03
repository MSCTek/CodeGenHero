using System;

namespace CodeGenHero.DataService
{
	public partial class WebApiExecutionContextType : IEquatable<WebApiExecutionContextType>, IWebApiExecutionContextType
	{
		private static readonly Lazy<WebApiExecutionContextType> _lazyInstance = new Lazy<WebApiExecutionContextType>(() => new WebApiExecutionContextType());
		protected int _current;

		public WebApiExecutionContextType()
		{
			_current = Base;
		}

		private enum _executionContextTypes
		{
			Base = 1
		}

		public static WebApiExecutionContextType Instance { get { return _lazyInstance.Value; } }
		public virtual int Base => (int)_executionContextTypes.Base;

		public virtual int Current
		{
			get
			{
				return _current;
			}
			set
			{
				switch (value)
				{
					case (int)_executionContextTypes.Base:
						_current = value;
						break;

					default:
						throw new ArgumentOutOfRangeException($"The value provided, {value}, for the current WebApiExecutionContextType is invalid.");
				}
			}
		}

		#region IEquatable

		public static bool operator !=(WebApiExecutionContextType objectA, WebApiExecutionContextType objectB)
		{
			return !(objectA == objectB);
		}

		public static bool operator ==(WebApiExecutionContextType objectA, WebApiExecutionContextType objectB)
		{
			// Check for null on left side.
			if (Object.ReferenceEquals(objectA, null))
			{
				if (Object.ReferenceEquals(objectB, null))
				{
					// null == null = true.
					return true;
				}

				// Only the left side is null.
				return false;
			}
			// Equals handles case of null on right side.
			return objectA.Equals(objectB);
		}

		public override bool Equals(object other)
		{
			return this.Equals(other as WebApiExecutionContextType);
		}

		public bool Equals(WebApiExecutionContextType other)
		{
			// If parameter is null, return false.
			if (Object.ReferenceEquals(other, null))
			{
				return false;
			}

			// Optimization for a common success case.
			if (Object.ReferenceEquals(this, other))
			{
				return true;
			}

			// If run-time types are not exactly the same, return false.
			if (this.GetType() != other.GetType())
			{
				return false;
			}

			// Return true if the fields match.
			// Note that the base class is not invoked because it is
			// System.Object, which defines Equals as reference equality.
			return (Current == other.Current);
		}

		public override int GetHashCode()
		{
			return Current.GetHashCode();
		}

		#endregion IEquatable
	}
}