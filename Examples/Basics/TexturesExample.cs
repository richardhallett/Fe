
namespace Fe.Examples.Basics
{
    class TexturesExample : IExample
    {

        public TexturesExample()
        {
            var cube = new Fe.Extra.Geometry.Cube();

            // Vertices that make up a cube.
            PosNormalTexCoordVertex[] vertices = new PosNormalTexCoordVertex[cube.Vertices.Length / 3];

            int i = 0;
            int offset = 0;
            for (i = 0; i < vertices.Length; i++)
            {
                vertices[i].x = cube.Vertices[offset];
                vertices[i].y = cube.Vertices[offset + 1];
                vertices[i].z = cube.Vertices[offset + 2];

                //vertices[i].normx = cube.Normals[offset];
                //vertices[i].normy = cube.Normals[offset + 1];
                //vertices[i].normz = cube.Normals[offset + 2];

                //vertices[i].texcoord0 = cube.Normals[offset];
                //vertices[i].texcoord1 = cube.Normals[offset + 1];

                vertices[i].abgr = 0xFF00FFFF;

                offset += 3;
            }            

            // Create vertex buffer for our cube
            this._vb = new Fe.VertexBuffer<PosNormalTexCoordVertex>(vertices);

            // Create index buffer
            this._ib = new Fe.IndexBuffer(cube.Indices);

            Fe.Uniform colourUniform = new Fe.Uniform("colour", Fe.UniformType.Uniform4f);
            this._ub = new Fe.UniformBuffer();
            this._ub.Set(colourUniform, 1.0f, 1.0f, 1.0f, 1.0f);
        }
        

        public void Update(CommandBucket commandBucket, ExampleData exampleData)
        {
            var cube = commandBucket.AddCommand(2);

            cube.VertexShader = exampleData.DefaultVertexShader;
            cube.FragmentShader = exampleData.DefaultFragmentShader;
            cube.VertexBuffer = _vb;
            cube.IndexBuffer = _ib;
            cube.SharedUniforms = _ub;

            cube.Transform = (Nml.Matrix4x4.Translate(new Nml.Vector3(z: -1.0f)) * Nml.Matrix4x4.RotateY(-0.5f)).ToArray();
        }

        private VertexBuffer<PosNormalTexCoordVertex> _vb;
        private IndexBuffer _ib;
        private UniformBuffer _ub;
    }
}
