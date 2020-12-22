using CodeGenHero.Core.Metadata.Interfaces;
using System;

namespace CodeGenHero.Core.Metadata
{
    [Serializable]
    public class EntityNavigation : IEntityNavigation
    {
        public EntityNavigation()
        {
        }

        public IEntityType EntityType { get; set; }

        public INavigation Navigation { get; set; }
    }
}