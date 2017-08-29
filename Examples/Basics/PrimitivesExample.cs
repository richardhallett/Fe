
using System.IO;

namespace Fe.Examples.Basics
{
    class PrimitivesExample : IExample
    {

        public PrimitivesExample(Renderer renderer)
        {
            // Create the shaders
            Fe.Shader vertexShader, fragmentShader;
            switch (renderer.GetRendererType())
            {
                case Fe.RendererType.OpenGL:
                    vertexShader = new Fe.Shader(Fe.ShaderType.Vertex, File.ReadAllText("default.vert"));
                    fragmentShader = new Fe.Shader(Fe.ShaderType.Fragment, File.ReadAllText("default.frag"));
                    break;
                default:
                    throw new System.Exception("Unknown backend renderer type");
            }

            vs = vertexShader;
            fs = fragmentShader;

            // Vertices that make up a cube.
            PosColorVertex[] vertices =
            {
                new PosColorVertex() { x = 0, y = 0,  z = 0 },
                new PosColorVertex() { x = 0, y = 1,  z = 0 },
                new PosColorVertex() { x = 1, y = 0,  z = 0 },
                new PosColorVertex() { x = 1, y = 1,  z = 0 },
            };

            // Create vertex buffer for our cube
            this._vb = new Fe.VertexBuffer<PosColorVertex>(vertices);

            Fe.Uniform colourUniform = new Fe.Uniform("colour", Fe.UniformType.Uniform4f);
            defaultUniforms = new Fe.UniformBuffer();
            defaultUniforms.Set(colourUniform, 1.0f, 1.0f, 1.0f, 1.0f);
        }


        public void Update(CommandBucket commandBucket)
        {

            // Draw a single triangle
            var tri = commandBucket.AddCommand(0);
            tri.SetShader(vs);
            tri.SetShader(fs);
            tri.SetVertexBuffer(_vb);
            tri.SetPrimitiveType(PrimitiveType.Triangles);
            tri.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: -2.0f)).ToArray());
            tri.SetSharedUniforms(defaultUniforms);

            // Draw a square using triangle strips
            var triStrip = commandBucket.AddCommand(0);
            triStrip.SetShader(vs);
            triStrip.SetShader(fs);
            triStrip.SetVertexBuffer(_vb);
            triStrip.SetPrimitiveType(PrimitiveType.TriangleStrip);
            triStrip.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: -0.9f)).ToArray());
            triStrip.SetSharedUniforms(defaultUniforms);

            // Draw 2 vertical lines
            var lines = commandBucket.AddCommand(0);
            lines.SetShader(vs);
            lines.SetShader(fs);
            lines.SetVertexBuffer(_vb);
            lines.SetPrimitiveType(PrimitiveType.Lines);
            lines.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: 0.2f)).ToArray());
            lines.SetSharedUniforms(defaultUniforms);

            // Draw connected lines
            var lineStrip = commandBucket.AddCommand(0);
            lineStrip.SetShader(vs);
            lineStrip.SetShader(fs);
            lineStrip.SetVertexBuffer(_vb);
            lineStrip.SetPrimitiveType(PrimitiveType.LineStrip);
            lineStrip.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(y: -1.1f)).ToArray());
            lineStrip.SetSharedUniforms(defaultUniforms);

            // Draw points
            var points = commandBucket.AddCommand(0);
            points.SetShader(vs);
            points.SetShader(fs);
            points.SetVertexBuffer(_vb);
            points.SetPrimitiveType(PrimitiveType.Points);
            points.SetTransform(Nml.Matrix4x4.Translate(new Nml.Vector3(x: -1.2f, y: -1.1f)).ToArray());
            points.SetSharedUniforms(defaultUniforms);
        }

        private UniformBuffer defaultUniforms;
        private Shader vs;
        private Shader fs;
        private VertexBuffer<PosColorVertex> _vb;
    }
}
