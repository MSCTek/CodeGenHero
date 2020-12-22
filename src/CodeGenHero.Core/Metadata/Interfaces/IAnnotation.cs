namespace CodeGenHero.Core.Metadata.Interfaces
{
    /// <summary>
    ///     <para>
    ///         An arbitrary piece of metadata that can be stored on an object that implements <see cref="IAnnotatable" />.
    ///     </para>
    /// </summary>
    public interface IAnnotation
    {
        /// <summary>
        ///     Gets the key of this annotation.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Gets the value assigned to this annotation.
        /// </summary>
        object Value { get; set; }
    }
}