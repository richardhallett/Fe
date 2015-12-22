
namespace Fe.Extra.Geometry
{
    /// <summary>
    /// Plane geometry creation.
    /// </summary>
    public class Plane
    {
        /// <summary>
        /// Creates plane geometry.
        /// </summary>
        /// <param name="width">Width along X axis.</param>
        /// <param name="height">Height along Y axis.</param>
        /// <param name="widthSegments">Number of width segments.</param>
        /// <param name="heightSegments">Number of height segments.</param>
        public Plane(int width, int height, int widthSegments = 1, int heightSegments = 1)
        {
            Width = width;
            Height = height;
            WidthSegments = widthSegments;
            HeightSegments = heightSegments;

            // Grid dimensions
            int gridX = widthSegments;
            int gridY = heightSegments;
            int gridX1 = gridX + 1;
            int gridY1 = gridY + 1;

            // Setup structures for storing the plane data
            Vertices = new float[gridX1 * gridY1 * 3];
            UVs = new float[gridX1 * gridY1 * 2];
            Normals = new float[gridX1 * gridY1 * 3];
            Indices = new uint[gridX * gridY * 6] ;

            // Width/height of each segment within the grid
            float segmentWidth = width / gridX;
            float segmentHeight = height / gridY;

            // Calculate half width's to be used so we build vertices from centre out
            float halfWidth = width / 2;
            float halfHeight = height / 2;

            int index = 0;
            int index2 = 0;
            for (var iy = 0; iy < gridY1; iy++)
            {
                var y = iy * segmentHeight - halfHeight;
                for (var ix = 0; ix < gridX1; ix++)
                {
                    var x = ix * segmentWidth - halfWidth;

                    Vertices[index] = x;
                    Vertices[index + 1] = -y;
                    
                    UVs[index2] = ix / gridX;
                    UVs[index2 + 1] = 1 - (iy / gridY);

                    Normals[index + 2] = 1; // Pointing directly out of the Z

                    index += 3;
                    index2 += 2;
                }
            }

            index = 0;
            for (uint iy = 0; iy < gridY; iy++)
            {
                for (uint ix = 0; ix < gridX; ix++)
                {
                    uint a = ix + (uint)gridX1 * iy;
                    uint b = ix + (uint)gridX1 * (iy + 1);
                    uint c = (ix + 1) + (uint)gridX1 * (iy + 1);
                    uint d = (ix + 1) + (uint)gridX1 * iy;

                    // Two triangles per grid
                    Indices[index] = a;
                    Indices[index + 1] = b;
                    Indices[index + 2] = d;

                    Indices[index + 3] = b;
                    Indices[index + 4] = c;
                    Indices[index + 5] = d;

                    index += 6;

                }
            }

        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the width segments.
        /// </summary>
        /// <value>
        /// The width segments.
        /// </value>
        public int WidthSegments { get; private set; }

        /// <summary>
        /// Gets the height segments.
        /// </summary>
        /// <value>
        /// The height segments.
        /// </value>
        public int HeightSegments { get; private set; }

        /// <summary>
        /// Gets the vertices.
        /// </summary>
        /// <value>
        /// The vertices.
        /// </value>
        public float[] Vertices { get; private set; }

        /// <summary>
        /// Gets the u vs.
        /// </summary>
        /// <value>
        /// The u vs.
        /// </value>
        public float[] UVs { get; private set; }

        /// <summary>
        /// Gets the normals.
        /// </summary>
        /// <value>
        /// The normals.
        /// </value>
        public float[] Normals { get; private set; }

        /// <summary>
        /// Gets the indices.
        /// </summary>
        /// <value>
        /// The indices.
        /// </value>
        public uint[] Indices { get; private set; }

    }
}
