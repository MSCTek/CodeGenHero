using System.Collections.Generic;

namespace CodeGenHero.Core.Metadata.Interfaces
{
    /// <summary>
    ///     Represents a primary or alternate key on an entity.
    /// </summary>
    public interface IKey : IAnnotatable
    {
        /// <summary>
        ///     Gets the entity type the key is defined on. This may be different from the type that <see cref="Properties" />
        ///     are defined on when the key is defined a derived type in an inheritance hierarchy (since the properties
        ///     may be defined on a base type).
        /// </summary>
        IEntityType DeclaringEntityType { get; set; }

        /// <summary>
        ///     Gets the properties that make up the key.
        /// </summary>
        IList<IProperty> Properties { get; set; }
    }
}