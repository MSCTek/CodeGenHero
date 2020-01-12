/// <summary>
///
/// </summary>
/// <remarks>
/// For original source see: https://raw.githubusercontent.com/Emilien-M/IListExtension/master/src/IListExtension/FunctorComparer.cs
/// </remarks>
namespace System.Collections.Generic
{
	internal sealed class FunctorComparer<T> : IComparer<T>
	{
		private readonly Comparison<T> comparison;

		public FunctorComparer(Comparison<T> comparison)
		{
			this.comparison = comparison;
		}

		public int Compare(T x, T y)
		{
			return comparison(x, y);
		}
	}
}