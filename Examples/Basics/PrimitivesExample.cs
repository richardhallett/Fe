
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
            tri.SetShader(exampleData.DefaultVertexShader);
            tri.SetShader(exampleData.DefaultFragmentShader);
            tri.SetVertexBuffer(_vb);
            tri.SetPrimitiveType(PrimitiveType.Triangles);
            tri.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: -2.0f)).ToArray());
            tri.SetSharedUniforms(exampleData.DefaultUniforms);

            // Draw a square using triangle strips
            var triStrip = commandBucket.AddCommand(0);
            triStrip.SetShader(exampleData.DefaultVertexShader);
            triStrip.SetShader(exampleData.DefaultFragmentShader);
            triStrip.SetVertexBuffer(_vb);
            triStrip.SetPrimitiveType(PrimitiveType.TriangleStrip);
            triStrip.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: -0.9f)).ToArray());

            // Draw 2 vertical lines
            var lines = commandBucket.AddCommand(0);
            lines.SetShader(exampleData.DefaultVertexShader);
            lines.SetShader(exampleData.DefaultFragmentShader);
            lines.SetVertexBuffer(_vb);
            lines.SetPrimitiveType(PrimitiveType.Lines);
            lines.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: 0.2f)).ToArray());

            // Draw connected lines
            var lineStrip = commandBucket.AddCommand(0);
            lineStrip.SetShader(exampleData.DefaultVertexShader);
            lineStrip.SetShader(exampleData.DefaultFragmentShader);
            lineStrip.SetVertexBuffer(_vb);
            lineStrip.SetPrimitiveType(PrimitiveType.LineStrip);
            lineStrip.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(y: -1.1f)).ToArray());

            // Draw points
            var points = commandBucket.AddCommand(0);
            points.SetShader(exampleData.DefaultVertexShader);
            points.SetShader(exampleData.DefaultFragmentShader);
            points.SetVertexBuffer(_vb);
            points.SetPrimitiveType(PrimitiveType.Points);
            points.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: -1.2f, y: -1.1f)).ToArray());
        }

        private VertexBuffer<PosNormalTexCoordVertex> _vb;
    }
}
