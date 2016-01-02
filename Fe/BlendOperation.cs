
namespace Fe
{
    /// <summary>
    /// RGB or Alpha blending operation.
    /// </summary>
    public enum BlendOperation
    {
        /// <summary>
        /// The source and destination colours are added to each other.
        /// </summary>
        Add,
        /// <summary>
        /// Subtract destination from the source.
        /// </summary>
        Subtract,
        /// <summary>
        /// Subtract the source from the destination.
        /// </summary>
        ReverseSubtract,
        /// <summary>
        /// Minimum of source and destination.
        /// </summary>
        Min,
        /// <summary>
        /// Maximum of source and destination.
        /// </summary>
        Max
    }
}
