using System.Collections.Generic;

namespace CodeGenHero.Core.Metadata.Interfaces
{
    public interface IForeignKey
    {
        IEntityType DeclaringEntityType { get; set; }

        /// <summary>
        ///     Gets the navigation property on the dependent entity type that points to the principal entity.
        /// </summary>
        INavigation DependentToPrincipal { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this relationship defines an ownership.
        ///     If <c>true</c>, the dependent entity must always be accessed via the navigation from the principal entity.
        /// </summary>
        bool IsOwnership { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this relationship is required.
        ///     If <c>true</c>, the dependent entity must always be assigned to a valid principal entity.
        /// </summary>
        bool IsRequired { get; set; }

        /// <summary>
        ///     Gets a value indicating whether the values assigned to the foreign key properties are unique.
        /// </summary>
        bool IsUnique { get; set; }

        /// <summary>
        ///     Gets the principal entity type that this relationship targets. This may be different from the type that
        ///     <see cref="PrincipalKey" /> is defined on when the relationship targets a derived type in an inheritance
        ///     hierarchy (since the key is defined on the base type of the hierarchy).
        /// </summary>
        IEntityType PrincipalEntityType { get; set; }

        /// <summary>
        ///     Gets the primary or alternate key that the relationship targets.
        /// </summary>
        IKey PrincipalKey { get; set; }

        /// <summary>
        ///     Gets the navigation property on the principal entity type that points to the dependent entity.
        /// </summary>
        INavigation PrincipalToDependent { get; set; }

        /// <summary>
        ///     Gets the foreign key properties in the dependent entity.
        /// </summary>
        IList<IProperty> Properties { get; set; }
    }
}