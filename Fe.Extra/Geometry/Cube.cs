
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
                -1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                -1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,
                -1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                -1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,
                -1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                -1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,
                -1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                -1.0f, -1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,
                -1.0f, -1.0f, -1.0f,
                -1.0f,  1.0f, -1.0f
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
                1.0f,  0.0f,  0.0f,
                1.0f,  0.0f,  0.0f,
                1.0f,  0.0f,  0.0f,
                1.0f,  0.0f,  0.0f
            };

            Indices = new uint[]
            {
                 0,  2,  1,
                 1,  2,  3,
                 4,  5,  6,
                 5,  7,  6,
                 8, 10,  9,
                 9, 10, 11,
                12, 13, 14,
                13, 15, 14,
                16, 18, 17,
                17, 18, 19,
                20, 21, 22,
                21, 23, 22,
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
