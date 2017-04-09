using System.Linq;
using ImageSharp;


namespace Fe.Examples.Basics
{
    class TexturesExample : IExample
    {
        Fe.Texture2d<Color> texture;
        Fe.Uniform colourMapUniform;

        public TexturesExample()
        {
            var cube = new Fe.Extra.Geometry.Cube();

            // Vertices that make up a cube.
            PosNormalTexCoordVertex[] vertices = new PosNormalTexCoordVertex[cube.Vertices.Length / 3];

            int i = 0;
            int vertOffset = 0;
            int texOffset = 0;
            for (i = 0; i < vertices.Length; i++)
            {
                vertices[i].x = cube.Vertices[vertOffset];
                vertices[i].y = cube.Vertices[vertOffset + 1];
                vertices[i].z = cube.Vertices[vertOffset + 2];

                vertices[i].normx = cube.Normals[vertOffset];
                vertices[i].normy = cube.Normals[vertOffset + 1];
                vertices[i].normz = cube.Normals[vertOffset + 2];

                vertices[i].texcoord0 = cube.UVs[texOffset];
                vertices[i].texcoord1 = cube.UVs[texOffset + 1];

                vertices[i].abgr = 0xFF00FFFF;

                vertOffset += 3;
                texOffset += 2;
            }

            // Create vertex buffer for our cube
            this._vb = new Fe.VertexBuffer<PosNormalTexCoordVertex>(vertices);

            // Create index buffer
            this._ib = new Fe.IndexBuffer(cube.Indices);

            colourMapUniform = new Fe.Uniform("colourMap", Fe.UniformType.Uniform1f);
            
            // Load example image we're going to apply
            using (Image image = Image.Load(@"..\assets\lilly.jpg"))
            {
                //var imageData = image.Pixels.Select(d => (byte)d.R).ToArray();

                texture = new Fe.Texture2d<Color>(image.Width, image.Height, SampleFormat.RGBA8, image.Pixels);
                //texture = new Fe.Texture2d<byte>(imageData, image.Width, image.Height, TextureFormat.R8);
            }
        }
        

        public void Update(CommandBucket commandBucket, ExampleData exampleData)
        {
            var cube = commandBucket.AddCommand(2);

            cube.VertexShader = exampleData.DefaultVertexShader;
            cube.FragmentShader = exampleData.DefaultFragmentShader;
            cube.VertexBuffer = _vb;
            cube.IndexBuffer = _ib;
            cube.SharedUniforms = _ub;
            cube.TextureStages[0].Set(texture, colourMapUniform);

            cube.Transform = (Nml.Matrix4x4.Translate(new Nml.Vector3(z: -1.0f)) * Nml.Matrix4x4.RotateY(-0.5f)).ToArray();
        }

        private VertexBuffer<PosNormalTexCoordVertex> _vb;
        private IndexBuffer _ib;
        private UniformBuffer _ub;
    }
}
