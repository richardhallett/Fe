
namespace Fe.Examples.Basics
{
    class TranslucencySortExample : IExample
    {

        public TranslucencySortExample()
        {
            // BlendState for alpha transparency.
            this._bs = new Fe.BlendState(true,
                Fe.BlendFactor.SourceAlpha,
                Fe.BlendFactor.InvertSourceAlpha,
                Fe.BlendOperation.Add,
                Fe.BlendFactor.One,
                Fe.BlendFactor.Zero,
                Fe.BlendOperation.Add);

            // Vertices that make up a cube.
            PosColorVertex[] vertices =
            {
                new PosColorVertex (-0.25f,  0.25f,  0.25f, 0xbbFF0000),
                new PosColorVertex ( 0.25f,  0.25f,  0.25f, 0xbbFF0000),
                new PosColorVertex (-0.25f, -0.25f,  0.25f, 0xbbFF0000),
                new PosColorVertex ( 0.25f, -0.25f,  0.25f, 0xbbFF0000),
                new PosColorVertex (-0.25f,  0.25f, -0.25f, 0xbbFF0000),
                new PosColorVertex ( 0.25f,  0.25f, -0.25f, 0xbbFF0000),
                new PosColorVertex (-0.25f, -0.25f, -0.25f, 0xbbFF0000),
                new PosColorVertex ( 0.25f, -0.25f, -0.25f, 0xbbFF0000),
            };

            // Indices that make up a cube.
            uint[] indexPositions =
            {
                0, 1, 2,
                1, 3, 2,
                4, 6, 5,
                5, 6, 7,
                0, 2, 4,
                4, 2, 6,
                1, 5, 3,
                5, 7, 3,
                0, 4, 1,
                4, 5, 1,
                2, 3, 6,
                6, 3, 7,
            };

            // Create vertex buffer for our cube
            this._vb = new Fe.VertexBuffer<PosColorVertex>(vertices);
            this._ib = new Fe.IndexBuffer(indexPositions);

            Fe.Uniform colourUniform = new Fe.Uniform("colour", Fe.UniformType.Uniform4f);
            _opaque = new Fe.UniformBuffer();
            _opaque.Set(colourUniform, 0.0f, 1.0f, 0.0f, 1.0f);

            _translucent = new Fe.UniformBuffer();
            _translucent.Set(colourUniform, 1.0f, 0.0f, 0.0f, 0.4f);
        }


        public void Update(CommandBucket commandBucket, ExampleData exampleData)
        {
            // Draw the translucent cube second
            var transluscentCube = commandBucket.AddCommand(2);

            transluscentCube.SetShader(exampleData.DefaultVertexShader);
            transluscentCube.SetShader(exampleData.DefaultFragmentShader);
            transluscentCube.SetBlendState(_bs);
            transluscentCube.SetVertexBuffer(_vb);
            transluscentCube.SetIndexBuffer(_ib);
            transluscentCube.SetSharedUniforms(_translucent);

            transluscentCube.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(z: 1.0f)).ToArray());
            
            // Draw the opaque cube first
            var opaqueCube = commandBucket.AddCommand(1);

            opaqueCube.SetShader(exampleData.DefaultVertexShader);
            opaqueCube.SetShader(exampleData.DefaultFragmentShader);
            opaqueCube.SetVertexBuffer(_vb);
            opaqueCube.SetIndexBuffer(_ib);
            opaqueCube.SetSharedUniforms(_opaque);

            opaqueCube.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: -0.4f)).ToArray());
        }

        private VertexBuffer<PosColorVertex> _vb;
        private IndexBuffer _ib;
        private BlendState _bs;
        private UniformBuffer _opaque;
        private UniformBuffer _translucent;
    }
}
