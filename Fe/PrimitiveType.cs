
namespace Fe
{
    /// <summary>
    /// Describes the topology of primitives when passed to the rasteriser.
    /// </summary>
    public enum PrimitiveType
    {
        /// <summary>
        /// List of isolated triangles.
        /// </summary>
        Triangles = 0,
        /// <summary>
        /// List of connected triangles.
        /// </summary>
        TriangleStrip = 1,
        /// <summary>
        /// List of isolated lines.
        /// </summary>
        Lines = 2,
        /// <summary>
        /// List of connected lines.
        /// </summary>
        LineStrip = 3,
        /// <summary>
        /// List of points.
        /// </summary>
        Points = 4
    }
}
