namespace CodeGenHero.Core.Metadata.Interfaces
{
    public interface IEntityNavigation
    {
        /// <summary>
        /// The declaring entity that this navigation belongs to
        /// </summary>
        IEntityType EntityType { get; set; }

        /// <summary>
        /// The navigation property of the entity type
        /// </summary>
        INavigation Navigation { get; set; }
    }
}