using JetBrains.Annotations;
using System.Collections.Generic;

namespace CodeGenHero.Core.Metadata.Interfaces
{
    public interface IModel
    {
        /// <summary>
        ///     Gets all entity types defined in the model.
        /// </summary>
        /// <returns> All entity types defined in the model. </returns>
        IList<IEntityType> EntityTypes { get; }

        string ModelFullyQualifiedTypeName { get; set; }
        string ModelTypeName { get; set; }

        /// <summary>
        ///     Gets the entity type with the given name. Returns null if no entity type with the given name is found
        ///     or the entity type has a defining navigation.
        /// </summary>
        /// <param name="name"> The name of the entity type to find. </param>
        /// <returns> The entity type, or null if none are found. </returns>
        IEntityType FindEntityType([NotNull] string name);

        /// <summary>
        ///     Gets the entity type for the given name, defining navigation name
        ///     and the defining entity type. Returns null if no matching entity type is found.
        /// </summary>
        /// <param name="name"> The name of the entity type to find. </param>
        /// <param name="definingNavigationName"> The defining navigation of the entity type to find. </param>
        /// <param name="definingEntityType"> The defining entity type of the entity type to find. </param>
        /// <returns> The entity type, or null if none are found. </returns>
        IEntityType FindEntityType(
            [NotNull] string name,
            [NotNull] string definingNavigationName,
            [NotNull] IEntityType definingEntityType);

        /// <summary>
        ///     Gets all entity types defined in the model.
        /// </summary>
        /// <returns> All entity types defined in the model. </returns>
        IList<IEntityType> GetEntityTypes();

        IList<IEntityType> GetEntityTypesByRegEx(string excludeRegExPattern, string includeRegExPattern);

        IList<IEntityType> GetEntityTypesFilteredByRegEx(string regExPattern);

        IList<IEntityType> GetEntityTypesMatchingRegEx(string regExPattern);
    }
}