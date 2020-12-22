using CodeGenHero.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Core.Metadata
{
    [Serializable]
    public class ForeignKey : MetadataBase, IForeignKey
    {
        public IEntityType DeclaringEntityType { get; set; }
        public INavigation DependentToPrincipal { get; set; }
        public bool IsOwnership { get; set; }
        public bool IsRequired { get; set; }
        public bool IsUnique { get; set; }
        public IEntityType PrincipalEntityType { get; set; }
        public IKey PrincipalKey { get; set; }
        public INavigation PrincipalToDependent { get; set; }
        public IList<IProperty> Properties { get; set; } = new List<IProperty>();
    }
}