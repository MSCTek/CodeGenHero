using CodeGenHero.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Core.Metadata
{
    [Serializable]
    public class Key : MetadataBase, IKey
    {
        public IEntityType DeclaringEntityType { get; set; }
        public IList<IProperty> Properties { get; set; } = new List<IProperty>();
    }
}