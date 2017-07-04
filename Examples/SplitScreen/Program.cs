using System;
using System.Diagnostics;
using System.IO;

namespace Fe.Examples.SplitScreen
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
            // Setup a renderering window, this can be handled with any windowing/platform library
            var canvas = new ExampleBase.GraphicsCanvas();

            // Create the renderer
            var renderer = new Fe.Renderer();

            // Set the handle from our form so we let Fe create context/devices as appropriate.
            renderer.SetWindowHandle(canvas.Handle);
            // Initialise defaults
            renderer.Init();

            // Create the shaders
            Fe.Shader vertexShader, fragmentShader;
            switch(renderer.GetRendererType()) 
            {                
                case Fe.RendererType.OpenGL:
                    vertexShader = new Fe.Shader(Fe.ShaderType.Vertex, File.ReadAllText("splitscreen.vert"));
                    fragmentShader = new Fe.Shader(Fe.ShaderType.Fragment, File.ReadAllText("splitscreen.frag"));
                    break;
                default:
                    throw new Exception("Unknown backend renderer type");
            }
            
            // Vertices that make up a cube.
            PosColorVertex[] vertices =
            {
                new PosColorVertex (-0.25f,  0.25f,  0.25f, 0xff000000 ),
                new PosColorVertex ( 0.25f,  0.25f,  0.25f, 0xff0000ff ),
                new PosColorVertex (-0.25f, -0.25f,  0.25f, 0xff00ff00 ),
                new PosColorVertex ( 0.25f, -0.25f,  0.25f, 0xff00ffff ),
                new PosColorVertex (-0.25f,  0.25f, -0.25f, 0xffff0000 ),
                new PosColorVertex ( 0.25f,  0.25f, -0.25f, 0xffff00ff ),
                new PosColorVertex (-0.25f, -0.25f, -0.25f, 0xffffff00 ),
                new PosColorVertex ( 0.25f, -0.25f, -0.25f, 0xffffffff ),
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
            var vb = new Fe.VertexBuffer<PosColorVertex>(vertices);
            var ib = new Fe.IndexBuffer(indexPositions);            

            void Resize(int width, int height)
            {
                var view1 = new Fe.View(0, height / 2, width, height / 2, true);
                view1.ClearColour = new Fe.Colour4(1.0f, 1.0f, 1.0f);
                var view2 = new Fe.View(0, 0, width, height / 2, true); 

                renderer.SetView(0, view1);
                renderer.SetView(1, view2);

                // Set up a projection matrix
                var projectionMatrix = Nml.Matrix4x4.PerspectiveProjectionRH(Nml.Common.Pi / 4, (float)width / ((float)height / 2), 0.1f, 100.0f);
                projectionMatrix *= Nml.Matrix4x4.Translate(0.0f, 0.0f, -5.0f);

                view1.SetTransform(Nml.Matrix4x4.Identity.ToArray(), projectionMatrix.ToArray());
                projectionMatrix *= Nml.Matrix4x4.Translate(0.0f, 0.0f, -5.0f);
                view2.SetTransform(Nml.Matrix4x4.Identity.ToArray(), projectionMatrix.ToArray());

                renderer.Reset(width, height);
            }

            // Force an Initial resize, you could handle this with some event on your window if you wanted.
            Resize(canvas.Width, canvas.Height);

            void Cleanup()
            {
                // Kill off the renderer and clean up all underlying resources.
                renderer.Dispose();
            }

            var view1Bucket = renderer.AddCommandBucket(UInt16.MaxValue, 0);
            var view2Bucket = renderer.AddCommandBucket(UInt16.MaxValue, 1);
            
            // Create some timers and run the main loop
            Stopwatch frameTimer = Stopwatch.StartNew();
            double frameTime = 0;
            float rotY = 0.0f;

            void Update()
            {
                frameTime = frameTimer.Elapsed.TotalMilliseconds;
                frameTimer.Restart();

                for (int i = 0; i < 5; i++ )
                {
                    var cubeCommand = view1Bucket.AddCommand(1);
                    cubeCommand.SetShader(vertexShader);
                    cubeCommand.SetShader(fragmentShader);
                    cubeCommand.SetVertexBuffer(vb);
                    cubeCommand.SetIndexBuffer(ib);

                    // Spin one way
                    cubeCommand.SetTransform((Nml.Matrix4x4.Translate(-6.0f + i * 3.0f, 0.0f, 0.0f) * Nml.Matrix4x4.RotateY(rotY + i * 0.32f)).ToArray());

                    var cubeCommand2 = view2Bucket.AddCommand(1);
                    cubeCommand2.SetShader(vertexShader);
                    cubeCommand2.SetShader(fragmentShader);
                    cubeCommand2.SetVertexBuffer(vb);
                    cubeCommand2.SetIndexBuffer(ib);

                    // Spin the other way
                    cubeCommand2.SetTransform((Nml.Matrix4x4.Translate(-6.0f + i * 3.0f, 0.0f, 0.0f) * Nml.Matrix4x4.RotateY(-rotY + i * 0.32f)).ToArray());
                     
                }                
                
                renderer.EndFrame();

                rotY += 0.001f;
            }

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
