using CodeGenHero.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenHero.Core
{
    public class PropertyListComparer : IComparer<IList<IProperty>>, IEqualityComparer<IList<IProperty>>
    {
        public static readonly PropertyListComparer Instance = new PropertyListComparer();

        private PropertyListComparer()
        {
        }

        public int Compare(IList<IProperty> x, IList<IProperty> y)
        {
            var result = x.Count - y.Count;

            if (result != 0)
            {
                return result;
            }

            var index = 0;
            while ((result == 0)
                   && (index < x.Count))
            {
                result = StringComparer.Ordinal.Compare(x[index].Name, y[index].Name);
                index++;
            }

            return result;
        }

        public bool Equals(IList<IProperty> x, IList<IProperty> y)
            => Compare(x, y) == 0;

        public int GetHashCode(IList<IProperty> obj)
            => obj.Aggregate(0, (hash, p) => unchecked((hash * 397) ^ p.GetHashCode()));
    }
}