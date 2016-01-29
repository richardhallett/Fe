
namespace Fe.Examples.Basics
{
    class PrimitivesExample : IExample
    {

        public PrimitivesExample()
        {
            // Vertices that make up a cube.
            PosColorVertex[] vertices =
            {
                new PosColorVertex (0,  0,  0),
                new PosColorVertex (0,  1,  0),
                new PosColorVertex (1,  0,  0),
                new PosColorVertex (1,  1,  0),
            };

            // Create vertex buffer for our cube
            this._vb = new Fe.VertexBuffer<PosColorVertex>(vertices);
        }
        

        public void Update(CommandBucket commandBucket, ExampleData exampleData)
        {

            // Draw a single triangle
            var tri = commandBucket.AddCommand(0);
            tri.SetShaderProgram(exampleData.DefaultProgram);
            tri.SetVertexBuffer(_vb);
            tri.SetPrimitiveType(PrimitiveType.Triangles);
            tri.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x:-2.0f)));
            tri.SetUniformBuffer(exampleData.DefaultUniforms);

            // Draw a square using triangle strips
            var triStrip = commandBucket.AddCommand(0);
            triStrip.SetShaderProgram(exampleData.DefaultProgram);
            triStrip.SetVertexBuffer(_vb);
            triStrip.SetPrimitiveType(PrimitiveType.TriangleStrip);
            triStrip.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: -0.9f)));

            // Draw 2 vertical lines
            var lines = commandBucket.AddCommand(0);
            lines.SetShaderProgram(exampleData.DefaultProgram);
            lines.SetVertexBuffer(_vb);
            lines.SetPrimitiveType(PrimitiveType.Lines);
            lines.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: 0.2f)));

            // Draw connected lines
            var lineStrip = commandBucket.AddCommand(0);
            lineStrip.SetShaderProgram(exampleData.DefaultProgram);
            lineStrip.SetVertexBuffer(_vb);
            lineStrip.SetPrimitiveType(PrimitiveType.LineStrip);
            lineStrip.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(y: -1.1f)));

            // Draw points
            var points = commandBucket.AddCommand(0);
            points.SetShaderProgram(exampleData.DefaultProgram);
            points.SetVertexBuffer(_vb);
            points.SetPrimitiveType(PrimitiveType.Points);
            points.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: -1.2f, y: -1.1f)));
        }

        private VertexBuffer<PosColorVertex> _vb;
    }
}
