using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StressTest1
{
    class Program
    {
        // Define a structure for our vertices.
        struct PosColorVertex : Fe.IVertex
        {
            public float x;
            public float y;
            public float z;
            public uint abgr;

            public PosColorVertex(float x, float y, float z, uint abgr)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.abgr = abgr;
            }

            private static Fe.VertexDeclaration vertexDeclaration = new Fe.VertexDeclaration
            (
                new Fe.VertexAttribute[] {
                        new Fe.VertexAttribute("Position", 3, Fe.VertexAttributeType.Float),
                        new Fe.VertexAttribute("Colour", 4, Fe.VertexAttributeType.Byte, true)
                    }
            );

            Fe.VertexDeclaration Fe.IVertex.VertexDeclaration { get { return vertexDeclaration; } }

        };

        static void Main(string[] args)
        {
            var form = new Fe.Forms.GraphicsForm();

            // Create the renderer
            var renderer = new Fe.Renderer();

            // Set the handle from our form so we let Fe create context/devices as appropriate.
            renderer.SetWindowHandle(form.Handle);

            // Initialise the renderer
            renderer.Init();

            // Create geometry layer command bucker
            var geometryBucket = renderer.AddCommandBucket(UInt16.MaxValue);

            // Create the shaders
            Fe.Shader vertexShader, fragmentShader;
            switch (renderer.GetRendererType())
            {
                case Fe.RendererType.OpenGL:
                    using (StreamReader fragReader = new StreamReader("default.frag"))
                    using (StreamReader vertReader = new StreamReader("default.vert"))
                    {
                        vertexShader = new Fe.Shader(Fe.ShaderType.Vertex, vertReader.ReadToEnd());
                        fragmentShader = new Fe.Shader(Fe.ShaderType.Fragment, fragReader.ReadToEnd());
                    }
                    break;
                default:
                    throw new Exception("Unknown backend renderer type");
            }

            // Link shaders into a program for binding.
            var shaderProgram = new Fe.ShaderProgram(new Fe.Shader[] { vertexShader, fragmentShader });

            // Build plane data
            var plane = new Fe.Extra.Geometry.Plane(30, 30, 30, 30);

            PosColorVertex[] vertices = new PosColorVertex[plane.Vertices.Length / 3];

            Random rand = new Random();
            int i = 0;
            int offset = 0;
            for(i = 0; i < vertices.Length; i++)
            {
                vertices[i].x = plane.Vertices[offset];
                vertices[i].y = plane.Vertices[offset + 1];
                vertices[i].z = plane.Vertices[offset + 2];
                vertices[i].abgr = (uint)(rand.Next(1 << 30)) << 2 | (uint)(rand.Next(1 << 2));;

                offset += 3;
            }
     
            // Create vertex buffer for our cube
            var vb = new Fe.VertexBuffer<PosColorVertex>(vertices, true);
            var ib = new Fe.IndexBuffer(plane.Indices);

            // Create shared uniforms
            Fe.Uniform projectionUniform = new Fe.Uniform("projectionMatrix", Fe.UniformType.Matrix4x4f);

            Fe.UniformBuffer sharedUniforms = new Fe.UniformBuffer();

            Stopwatch frameTimer = Stopwatch.StartNew();
            double frameTime = 0;
            float rotTime = 0.0f;
            float waveyTime = 0.0f;

            form.Text = "Dynamic vertex buffer update example";

            form.Resize += (object o, EventArgs e) =>
            {
                // Set up a projection matrix
                var projectionMatrix = Nml.Matrix4x4.PerspectiveProjectionRH(Nml.Common.Pi / 4, (float)form.Width / (float)form.Height, 0.1f, 100.0f);
                projectionMatrix *= Nml.Matrix4x4.Translate(0, 0, -50.0f);
                sharedUniforms.Set(projectionUniform, projectionMatrix);

                renderer.Reset(form.Width, form.Height);
            };

            form.FormClosing += (object o, FormClosingEventArgs e) =>
            {
                // Kill off the renderer and clean up all underlying resources.
                renderer.Dispose();
            };

            Fe.Forms.Application.Run(form, () =>
            {
                frameTime = frameTimer.Elapsed.TotalMilliseconds;
                frameTimer.Restart();

                // Increase the animation
                rotTime += (float)frameTime * 0.0010f;
                waveyTime += (float)frameTime * 0.01f;

                var cubeCommand = geometryBucket.AddCommand(1);

                cubeCommand.SetShaderProgram(shaderProgram);
                cubeCommand.SetVertexBuffer(vb);
                cubeCommand.SetIndexBuffer(ib);
                cubeCommand.SetUniformBuffer(sharedUniforms);

                // Generate new vertice positions
                for (i = 0; i < vertices.Length; i++)
                {
                    float newPos = (float)Math.Sin(waveyTime + i) * 1.5f;
                    vertices[i].z = newPos;
                }

                // Update the buffer
                vb.SetData(vertices);

                Nml.Matrix4x4 planeTransform = Nml.Matrix4x4.Identity;
                // Rotate it to make look pretty    
                Nml.Quaternion rotQuat;
                Nml.Quaternion.RotateEuler(0, rotTime * 0.37f, rotTime * 0.13f, out rotQuat);
                Nml.Quaternion.GetMatrix4x4(ref rotQuat, out planeTransform);

                cubeCommand.SetTransform(planeTransform);

                // Submit current commands queued to the renderer for rendering.
                renderer.EndFrame();
            });
        }
    }
}
