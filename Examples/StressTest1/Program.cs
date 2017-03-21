using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Fe.Examples.StressTest1
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

            // Initialise the renderer
            renderer.Init();

            // Create geometry layer command bucker
            var geometryBucket = renderer.AddCommandBucket(UInt16.MaxValue);

            // Create the shaders
            Fe.Shader vertexShader, fragmentShader;
            switch (renderer.GetRendererType())
            {
                
                case Fe.RendererType.OpenGL:                    
                    vertexShader = new Fe.Shader(Fe.ShaderType.Vertex, File.ReadAllText("stresstest.vert"));
                    fragmentShader = new Fe.Shader(Fe.ShaderType.Fragment, File.ReadAllText("stresstest.frag"));
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

            // Specify number of cube dimensions
            int dim = 16;
            int totalCubeCount = dim * dim * dim;

            // Threshold for the timings
            const double highThreshold = 1000 / 65;
            const double lowThreshold = 1000 / 57;

            float animTime = 0.0f; // Use for animating the cubes           

            // Create some timers and run the main loop
            Stopwatch frameTimer = Stopwatch.StartNew();
            int frameCount = 0;
            double frameTime = 0;
            double frameTimeAccum = 0;
            double averageFrameTime = 0;

            void Resize(int width, int height)
            {
                var view = new Fe.View(0, 0, width, height);
                renderer.SetView(0, view);

                // Set up a projection matrix
                var projectionMatrix = Nml.Matrix4x4.PerspectiveProjectionRH(Nml.Common.Pi / 4, (float)width / (float)height, 0.1f, 100.0f);
                projectionMatrix *= Nml.Matrix4x4.Translate(-5.0f, -5.0f, -50.0f);

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
                frameTime = frameTimer.Elapsed.TotalMilliseconds;
                frameTimer.Restart();

                frameTimeAccum += frameTime;
                // Increase/Decrease dimensions of cubes based upon average frame time
                if (frameTimeAccum >= 1000)
                {
                    averageFrameTime = frameTimeAccum / frameCount;
                    if (averageFrameTime < highThreshold)
                    {
                        dim = dim + 2;
                    }
                    else if (averageFrameTime > lowThreshold)
                    {
                        dim = Math.Max(dim - 1, 2);
                    }

                    frameCount = 0;
                    frameTimeAccum = 0;

                    canvas.Title = String.Format("Cube count: {0}, Avg Frametime: {1:N2} , Avg FPS: {2:N0}", totalCubeCount, averageFrameTime, 1000f / averageFrameTime);
                }
                frameCount++;

                // Starting point
                Nml.Vector3 initial = new Nml.Vector3(
                    -0.6f * dim / 2.0f,
                    -0.6f * dim / 2.0f,
                    -15.0f
                );

                totalCubeCount = dim * dim * dim;

                // Increase the animation
                animTime += (float)frameTime * 0.0010f;

                for (int x = 0; x < dim; x++)
                // Parallel.For(0, dim, x =>
                {
                    for (int y = 0; y < dim; y++)
                    {
                        for (int z = 0; z < dim; z++)
                        {
                            var cubeCommand = geometryBucket.AddCommand(1);

                            cubeCommand.VertexShader = vertexShader;
                            cubeCommand.FragmentShader = fragmentShader;
                            cubeCommand.VertexBuffer = vb;
                            cubeCommand.IndexBuffer = ib;

                            Nml.Matrix4x4 cubeTransform = Nml.Matrix4x4.Identity;

                            // Rotate it to make look pretty    
                            Nml.Quaternion rotQuat;
                            Nml.Quaternion.RotateEuler(animTime + x * 0.21f, animTime + y * 0.37f, animTime + y * 0.13f, out rotQuat);
                            Nml.Quaternion.GetMatrix4x4(ref rotQuat, out cubeTransform);

                            // Set translation part
                            cubeTransform.M14 = initial.x + x * 0.8f;
                            cubeTransform.M24 = initial.y + y * 0.8f;
                            cubeTransform.M34 = initial.z + z * 0.8f;

                            // Slightly faster to set the transform via the individual matrix components as we can avoid a memory allocation of a temporary array.
                            cubeCommand.SetTransformComponents(
                                cubeTransform.M11, cubeTransform.M12, cubeTransform.M13, cubeTransform.M14,
                                cubeTransform.M21, cubeTransform.M22, cubeTransform.M23, cubeTransform.M24,
                                cubeTransform.M31, cubeTransform.M32, cubeTransform.M33, cubeTransform.M34,
                                cubeTransform.M41, cubeTransform.M42, cubeTransform.M43, cubeTransform.M44);
                        }
                    }
                }
                //);

                // Submit current commands queued to the renderer for rendering.
                renderer.EndFrame();
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
