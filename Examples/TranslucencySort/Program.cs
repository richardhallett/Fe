using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fe.Examples.TranslucencySort
{
    class Program
    {
        // Define a structure for our vertices.
        struct PosVertex : Fe.IVertex
        {
            public float x;
            public float y;
            public float z;

            public PosVertex(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            private static Fe.VertexDeclaration vertexDeclaration = new Fe.VertexDeclaration
            (
                new Fe.VertexAttribute[] {
                        new Fe.VertexAttribute("Position", 3, Fe.VertexAttributeType.Float)
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
                    using (StreamReader fragReader = new StreamReader("translucency.frag"))
                    using (StreamReader vertReader = new StreamReader("translucency.vert"))
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
            // Vertices that make up a cube.
            PosVertex[] vertices =
            {
                new PosVertex (-0.25f,  0.25f,  0.25f),
                new PosVertex ( 0.25f,  0.25f,  0.25f),
                new PosVertex (-0.25f, -0.25f,  0.25f),
                new PosVertex ( 0.25f, -0.25f,  0.25f),
                new PosVertex (-0.25f,  0.25f, -0.25f),
                new PosVertex ( 0.25f,  0.25f, -0.25f),
                new PosVertex (-0.25f, -0.25f, -0.25f),
                new PosVertex ( 0.25f, -0.25f, -0.25f),
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
            var vb = new Fe.VertexBuffer<PosVertex>(vertices);
            var ib = new Fe.IndexBuffer(indexPositions);

            // BlendState for alpha transparency.
            var transparencyBlendState = new Fe.BlendState(true, 
                Fe.BlendFactor.SourceAlpha, 
                Fe.BlendFactor.InvertSourceAlpha, 
                Fe.BlendOperation.Add, 
                Fe.BlendFactor.One, 
                Fe.BlendFactor.Zero, 
                Fe.BlendOperation.Add);

            // Uniforms for different colour cubes.
            Fe.Uniform colourUniform = new Fe.Uniform("colour", Fe.UniformType.Uniform4f);
            Fe.UniformBuffer opaqueUniforms = new Fe.UniformBuffer();
            opaqueUniforms.Set(colourUniform, 0.0f, 1.0f, 0.0f, 1.0f);

            Fe.UniformBuffer translucentUniforms = new Fe.UniformBuffer();
            translucentUniforms.Set(colourUniform, 1.0f, 0.0f, 0.0f, 0.4f);
            
            form.Resize += (object o, EventArgs e) =>
            {
                var view = new Fe.View(0, 0, form.Width, form.Height);
                renderer.SetView(0, view);

                // Set up a projection matrix
                var projectionMatrix = Nml.Matrix4x4.PerspectiveProjectionRH(Nml.Common.Pi / 4, (float)form.Width / (float)form.Height, 0.1f, 100.0f);
                projectionMatrix *= Nml.Matrix4x4.Translate(0, 0, -3.0f);

                view.SetTransform(Nml.Matrix4x4.Identity, projectionMatrix);

                renderer.Reset(form.Width, form.Height);
            };

            form.FormClosing += (object o, FormClosingEventArgs e) =>
            {
                // Kill off the renderer and clean up all underlying resources.
                renderer.Dispose();
            };

            Fe.Forms.Application.Run(form, () =>
            {
                Nml.Matrix4x4 cubeTransform = Nml.Matrix4x4.Identity;

                // Draw the translucent cube second
                var transluscentCube = geometryBucket.AddCommand(2);

                transluscentCube.SetShaderProgram(shaderProgram);
                transluscentCube.SetBlendState(transparencyBlendState);
                transluscentCube.SetVertexBuffer(vb);
                transluscentCube.SetIndexBuffer(ib);
                transluscentCube.SetUniformBuffer(translucentUniforms);

                // Set translation part
                cubeTransform.M14 = 0.0f;
                cubeTransform.M24 = 0.0f;
                cubeTransform.M34 = 1.0f;

                transluscentCube.SetTransform(cubeTransform);

                // Draw the opaque cube first
                var opaqueCube = geometryBucket.AddCommand(1);

                opaqueCube.SetShaderProgram(shaderProgram);
                opaqueCube.SetVertexBuffer(vb);
                opaqueCube.SetIndexBuffer(ib);
                opaqueCube.SetUniformBuffer(opaqueUniforms);
                
                // Set translation part
                cubeTransform.M14 = -0.4f;
                cubeTransform.M24 = 0.0f;
                cubeTransform.M34 = 0.0f;

                opaqueCube.SetTransform(cubeTransform);

                // Submit current commands queued to the renderer for rendering.
                renderer.EndFrame();
            });
        }
    }
}
