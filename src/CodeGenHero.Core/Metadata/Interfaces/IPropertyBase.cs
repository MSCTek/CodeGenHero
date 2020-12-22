namespace CodeGenHero.Core.Metadata.Interfaces
{
    /// <summary>
    ///     Base type for navigation and scalar properties.
    /// </summary>
    public interface IPropertyBase : IAnnotatable
    {
        /// <summary>
        ///     Gets the type of value that this property holds.
        /// </summary>
        ClrType ClrType { get; set; }

        /// <summary>
        ///     Gets the type that this property belongs to.
        /// </summary>
        ITypeBase DeclaringType { get; set; }

        /// <summary>
        ///     Gets the name of the property.
        /// </summary>
        string Name { get; set; }
    }
}