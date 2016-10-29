
namespace Fe.Extra.Geometry
{
    /// <summary>
    /// Cube geometry creation.
    /// </summary>
    public class Cube
    {
        /// <summary>
        /// Creates plane geometry.
        /// </summary>
        public Cube()
        {
            Vertices = new float[]
            {
                // front
                -1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,
                // top
                -1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f, -1.0f,
                -1.0f,  1.0f, -1.0f,
                // back
                 1.0f, -1.0f, -1.0f,
                -1.0f, -1.0f, -1.0f,
                -1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                // bottom
                -1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f,  1.0f,
                -1.0f, -1.0f,  1.0f,
                // left
                -1.0f, -1.0f, -1.0f,
                -1.0f, -1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f, -1.0f,
                // right
                 1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f,  1.0f,
            };

            UVs = new float[]
            {
                0, 0,
                1, 0,
                0, 1,
                1, 1,

                0, 0,
                1, 0,
                0, 1,
                1, 1,

                0, 0,
                1, 0,
                0, 1,
                1, 1,

                0, 0,
                1, 0,
                0, 1,
                1, 1,

                0, 0,
                1, 0,
                0, 1,
                1, 1,

                0, 0,
                1, 0,
                0, 1,
                1, 1,
            };

            Normals = new float[]
            {
                0.0f,  0.0f,  1.0f,
                0.0f,  0.0f,  1.0f,
                0.0f,  0.0f,  1.0f,
                0.0f,  0.0f,  1.0f,

                0.0f,  0.0f, -1.0f,
                0.0f,  0.0f, -1.0f,
                0.0f,  0.0f, -1.0f,
                0.0f,  0.0f, -1.0f,

                0.0f,  1.0f,  0.0f,
                0.0f,  1.0f,  0.0f,
                0.0f,  1.0f,  0.0f,
                0.0f,  1.0f,  0.0f,

                0.0f, -1.0f,  0.0f,
                0.0f, -1.0f,  0.0f,
                0.0f, -1.0f,  0.0f,
                0.0f, -1.0f,  0.0f,

                1.0f,  0.0f,  0.0f,
                1.0f,  0.0f,  0.0f,
                1.0f,  0.0f,  0.0f,
                1.0f,  0.0f,  0.0f,

                -1.0f,  0.0f,  0.0f,
                -1.0f,  0.0f,  0.0f,
                -1.0f,  0.0f,  0.0f,
                -1.0f,  0.0f,  0.0f,
            };

            Indices = new uint[]
            {
                // front
                 0,  1,  2,
                 2,  3,  0,
                // top
                 4,  5,  6,
                 6,  7,  4,
                // back
                 8,  9, 10,
                10, 11,  8,
                // bottom
                12, 13, 14,
                14, 15, 12,
                // left
                16, 17, 18,
                18, 19, 16,
                // right
                20, 21, 22,
                22, 23, 20,
            };
            
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
