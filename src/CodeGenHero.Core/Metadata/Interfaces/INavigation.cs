namespace CodeGenHero.Core.Metadata.Interfaces
{
    /// <summary>
    ///     Represents a navigation property which can be used to navigate a relationship.
    /// </summary>
    public interface INavigation : IPropertyBase
    {
        /// <summary>
        ///     Gets the entity type that this property belongs to.
        /// </summary>
        IEntityType DeclaringEntityType { get; set; }

        /// <summary>
        ///     Gets the foreign key that defines the relationship this navigation property will navigate.
        /// </summary>
        IForeignKey ForeignKey { get; set; }
    }
}