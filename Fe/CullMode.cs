
namespace Fe
{
    /// <summary>
    /// The winding order for culling faces.
    /// </summary>
    public enum CullMode
    {
        /// <summary>
        /// Don't cull faces.
        /// </summary>
        None = 1,
        /// <summary>
        /// Cull faces with clockwise vertices.
        /// </summary>
        Clockwise = 2,
        /// <summary>
        /// Cull faces with counterclockwise vertices.
        /// </summary>
        CounterClockwise = 3,
    }
}
