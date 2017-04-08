using System;
using System.Collections.Generic;
using System.IO;

namespace Fe.Examples.Basics
{
    class Program
    {        
        static void Main(string[] args)
        {
            var canvas = new ExampleBase.GraphicsCanvas();

            // Create the renderer
            var renderer = new Fe.Renderer();

            // Set the handle from our form so we let Fe create context/devices as appropriate.
            renderer.SetWindowHandle(canvas.Handle);

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
                    vertexShader = new Fe.Shader(Fe.ShaderType.Vertex, File.ReadAllText("default.vert"));
                    fragmentShader = new Fe.Shader(Fe.ShaderType.Fragment, File.ReadAllText("default.frag"));
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
            var runningExampleIndex = 0;

            var examples = new List<IExample> {
                texturesExample,
                primitivesExample,
                translucencySortExample
            };

            void ChangeExample()
            {
                renderer.Reset();

                if (runningExampleIndex < examples.Count-1)
                {
                    runningExampleIndex += 1;
                } else
                {
                    runningExampleIndex = 0;
                }

                runningExample = examples[runningExampleIndex];
            }            

            void Resize(int width, int height)
            {
                var view = new Fe.View(0, 0, width, height);
                view.ClearColour = new Colour4(0x62799e);
                renderer.SetView(0, view);
                
                // Set up a projection matrix
                var projectionMatrix = Nml.Matrix4x4.PerspectiveProjectionRH(Nml.Common.Pi / 4, (float)width / (float)height, 0.1f, 100.0f);
                projectionMatrix *= Nml.Matrix4x4.Translate(0, 0, -3.0f);

                view.SetTransform(Nml.Matrix4x4.Identity.ToArray(), projectionMatrix.ToArray());

                renderer.Reset(width, height);
            }

            // Force an Initial resize, you could handle this with some event on your window if you wanted.
            Resize(canvas.Width, canvas.Height);

            void Cleanup()
            {
                // Kill off the renderer and clean up all underlying resources.
                renderer.Dispose();
            }

            void Update()
            {
                runningExample.Update(geometryBucket, exampleData);

                renderer.EndFrame();
            }

            canvas.MouseUp += () =>
            {
               // ChangeExample();
            };

            canvas.Resize += () =>
            {
                Resize(canvas.Width, canvas.Height);
            };

            canvas.Closing += () =>
            {
                Cleanup();
            };

            ExampleBase.Application.Run(canvas, () =>
            {
                Update();
            });
        }
    }
}
