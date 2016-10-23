
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
            tri.VertexShader = exampleData.DefaultVertexShader;
            tri.FragmentShader = exampleData.DefaultFragmentShader;
            tri.VertexBuffer = _vb;
            tri.PrimitiveType = PrimitiveType.Triangles;
            tri.Transform = Nml.Matrix4x4.Translate(new Nml.Vector3(x: -2.0f)).ToArray();
            tri.SharedUniforms = exampleData.DefaultUniforms;

            // Draw a square using triangle strips
            var triStrip = commandBucket.AddCommand(0);
            triStrip.VertexShader = exampleData.DefaultVertexShader;
            triStrip.FragmentShader = exampleData.DefaultFragmentShader;
            triStrip.VertexBuffer = _vb;
            triStrip.PrimitiveType = PrimitiveType.TriangleStrip;
            triStrip.Transform = Nml.Matrix4x4.Translate(new Nml.Vector3(x: -0.9f)).ToArray();

            // Draw 2 vertical lines
            var lines = commandBucket.AddCommand(0);
            lines.VertexShader = exampleData.DefaultVertexShader;
            lines.FragmentShader = exampleData.DefaultFragmentShader;
            lines.VertexBuffer = _vb;
            lines.PrimitiveType = PrimitiveType.Lines;
            lines.Transform = Nml.Matrix4x4.Translate(new Nml.Vector3(x: 0.2f)).ToArray();

            // Draw connected lines
            var lineStrip = commandBucket.AddCommand(0);
            lineStrip.VertexShader = exampleData.DefaultVertexShader;
            lineStrip.FragmentShader = exampleData.DefaultFragmentShader;
            lineStrip.VertexBuffer = _vb;
            lineStrip.PrimitiveType = PrimitiveType.LineStrip;
            lineStrip.Transform = Nml.Matrix4x4.Translate(new Nml.Vector3(y: -1.1f)).ToArray();

            // Draw points
            var points = commandBucket.AddCommand(0);
            points.VertexShader = exampleData.DefaultVertexShader;
            points.FragmentShader = exampleData.DefaultFragmentShader;
            points.VertexBuffer = _vb;
            points.PrimitiveType = PrimitiveType.Points;
            points.Transform = Nml.Matrix4x4.Translate(new Nml.Vector3(x: -1.2f, y: -1.1f)).ToArray();
        }

        private VertexBuffer<PosNormalTexCoordVertex> _vb;
    }
}
