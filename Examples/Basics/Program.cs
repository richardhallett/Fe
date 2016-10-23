using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fe.Examples.Basics
{
    class Program
    {        
        static void Main(string[] args)
        {
            var form = new Fe.Forms.GraphicsForm();

            // Create the renderer
            var renderer = new Fe.Renderer();

            // Set the handle from our form so we let Fe create context/devices as appropriate.
            renderer.SetWindowHandle(form.Handle);

            // Initialise the renderer
            renderer.Init();

            // Create Example Data object for storing generic data we'll use across examples
            ExampleData exampleData = new ExampleData();

            // Create geometry layer command bucket
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

            exampleData.DefaultVertexShader = vertexShader;
            exampleData.DefaultFragmentShader = fragmentShader;

            Fe.Uniform colourUniform = new Fe.Uniform("colour", Fe.UniformType.Uniform4f);
            var defaultUniforms = new Fe.UniformBuffer();
            defaultUniforms.Set(colourUniform, 1.0f, 1.0f, 1.0f, 1.0f);
            exampleData.DefaultUniforms = defaultUniforms;

            IExample runningExample;

            // Example to demonstrate different primitive topologies
            var primitivesExample = new PrimitivesExample();

            // Example to demonstrate transluencey with a sort
            var translucencySortExample = new TranslucencySortExample();

            // Example to demonstrate transluencey with a sort
            var texturesExample = new TexturesExample();

            // Default example to start with
            runningExample = texturesExample;

            // Key handling to switch examples
            form.KeyDown += (object o, KeyEventArgs e) =>
            {
                renderer.Reset();
                if (e.KeyCode == Keys.D1)
                {                    
                    runningExample = primitivesExample;
                }

                if (e.KeyCode == Keys.D2)
                {
                    runningExample = translucencySortExample;
                }

                if (e.KeyCode == Keys.D3)
                {
                    runningExample = texturesExample;
                }
            };

            form.Resize += (object o, EventArgs e) =>
            {
                var view = new Fe.View(0, 0, form.Width, form.Height);
                renderer.SetView(0, view);
                
                // Set up a projection matrix
                var projectionMatrix = Nml.Matrix4x4.PerspectiveProjectionRH(Nml.Common.Pi / 4, (float)form.Width / (float)form.Height, 0.1f, 100.0f);
                projectionMatrix *= Nml.Matrix4x4.Translate(0, 0, -3.0f);

                view.SetTransform(Nml.Matrix4x4.Identity.ToArray(), projectionMatrix.ToArray());

                renderer.Reset(form.Width, form.Height);
            };

            form.FormClosing += (object o, FormClosingEventArgs e) =>
            {
                // Kill off the renderer and clean up all underlying resources.
                renderer.Dispose();
            };

            Fe.Forms.Application.Run(form, () =>
            {
                runningExample.Update(geometryBucket, exampleData);

                renderer.EndFrame();
            });
        }
    }
}
