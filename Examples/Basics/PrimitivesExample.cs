
namespace Fe.Examples.Basics
{
    class PrimitivesExample : IExample
    {

        public PrimitivesExample()
        {
            // Vertices that make up a cube.
            PosNormalTexCoordVertex[] vertices =
            {
                new PosNormalTexCoordVertex() { x = 0, y = 0,  z = 0 },
                new PosNormalTexCoordVertex() { x = 0, y = 1,  z = 0 },
                new PosNormalTexCoordVertex() { x = 1, y = 0,  z = 0 },
                new PosNormalTexCoordVertex() { x = 1, y = 1,  z = 0 },
            };

            // Create vertex buffer for our cube
            this._vb = new Fe.VertexBuffer<PosNormalTexCoordVertex>(vertices);
        }
        

        public void Update(CommandBucket commandBucket, ExampleData exampleData)
        {

            // Draw a single triangle
            var tri = commandBucket.AddCommand(0);
            tri.SetVertexShader(exampleData.DefaultVertexShader);
            tri.SetFragmentShader(exampleData.DefaultFragmentShader);
            tri.SetVertexBuffer(_vb);
            tri.SetPrimitiveType(PrimitiveType.Triangles);
            tri.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x:-2.0f)));
            tri.SetUniformBuffer(exampleData.DefaultUniforms);

            // Draw a square using triangle strips
            var triStrip = commandBucket.AddCommand(0);
            triStrip.SetVertexShader(exampleData.DefaultVertexShader);
            triStrip.SetFragmentShader(exampleData.DefaultFragmentShader);
            triStrip.SetVertexBuffer(_vb);
            triStrip.SetPrimitiveType(PrimitiveType.TriangleStrip);
            triStrip.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: -0.9f)));

            // Draw 2 vertical lines
            var lines = commandBucket.AddCommand(0);
            lines.SetVertexShader(exampleData.DefaultVertexShader);
            lines.SetFragmentShader(exampleData.DefaultFragmentShader);
            lines.SetVertexBuffer(_vb);
            lines.SetPrimitiveType(PrimitiveType.Lines);
            lines.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: 0.2f)));

            // Draw connected lines
            var lineStrip = commandBucket.AddCommand(0);
            lineStrip.SetVertexShader(exampleData.DefaultVertexShader);
            lineStrip.SetFragmentShader(exampleData.DefaultFragmentShader);
            lineStrip.SetVertexBuffer(_vb);
            lineStrip.SetPrimitiveType(PrimitiveType.LineStrip);
            lineStrip.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(y: -1.1f)));

            // Draw points
            var points = commandBucket.AddCommand(0);
            points.SetVertexShader(exampleData.DefaultVertexShader);
            points.SetFragmentShader(exampleData.DefaultFragmentShader);
            points.SetVertexBuffer(_vb);
            points.SetPrimitiveType(PrimitiveType.Points);
            points.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: -1.2f, y: -1.1f)));
        }

        private VertexBuffer<PosNormalTexCoordVertex> _vb;
    }
}
