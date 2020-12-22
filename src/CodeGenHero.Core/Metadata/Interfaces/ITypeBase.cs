namespace CodeGenHero.Core.Metadata.Interfaces
{
    /// <summary>
    ///     Represents a type in an <see cref="IModel" />.
    /// </summary>
    public interface ITypeBase
    {
        /// <summary>
        ///     <para>
        ///         Gets the CLR class that is used to represent instances of this type.
        ///         Returns <c>null</c> if the type does not have a corresponding CLR class (known as a shadow type).
        ///     </para>
        /// </summary>
        /// <remarks>
        /// Cannot use "Type ClrType { get; set; }" because JSON.Net cannot deserialize abstract types.
        /// </remarks>
        ClrType ClrType { get; set; }

        /// <summary>
        ///     Gets the model that this type belongs to.
        /// </summary>
        //IModel Model { get; set; }

        /// <summary>
        ///     Gets the name of this type.
        /// </summary>
        string Name { get; set; }
    }
}