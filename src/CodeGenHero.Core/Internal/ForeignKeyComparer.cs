using CodeGenHero.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Core
{
    [Serializable]
    public class ForeignKeyComparer : IEqualityComparer<IForeignKey>, IComparer<IForeignKey>
    {
        public static readonly ForeignKeyComparer Instance = new ForeignKeyComparer();

        private ForeignKeyComparer()
        {
        }

        public virtual int Compare(IForeignKey x, IForeignKey y)
        {
            var result = PropertyListComparer.Instance.Compare(x.Properties, y.Properties);
            if (result != 0)
            {
                return result;
            }

            result = PropertyListComparer.Instance.Compare(x.PrincipalKey.Properties, y.PrincipalKey.Properties);
            if (result != 0)
            {
                return result;
            }

            result = EntityTypePathComparer.Instance.Compare(x.PrincipalEntityType, y.PrincipalEntityType);
            return result != 0 ? result : EntityTypePathComparer.Instance.Compare(x.DeclaringEntityType, y.DeclaringEntityType);
        }

        public virtual bool Equals(IForeignKey x, IForeignKey y)
            => Compare(x, y) == 0;

        public virtual int GetHashCode(IForeignKey obj) =>
            unchecked(
                ((((PropertyListComparer.Instance.GetHashCode(obj.PrincipalKey.Properties) * 397)
                   ^ PropertyListComparer.Instance.GetHashCode(obj.Properties)) * 397)
                 ^ EntityTypePathComparer.Instance.GetHashCode(obj.PrincipalEntityType)) * 397)
            ^ EntityTypePathComparer.Instance.GetHashCode(obj.DeclaringEntityType);
    }
}