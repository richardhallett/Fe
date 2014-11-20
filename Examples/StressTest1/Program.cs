using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
            // Initialise defaults
            renderer.Init();

            // Create the shaders
            Fe.Shader vertexShader, fragmentShader;
            switch(renderer.GetRendererType()) 
            {                
                case Fe.RendererType.OpenGL:                
                    using (StreamReader fragReader = new StreamReader("stresstest.frag"))
                    using (StreamReader vertReader = new StreamReader("stresstest.vert"))
                    {
                        vertexShader = new Fe.Shader(Fe.ShaderType.Vertex, vertReader.ReadToEnd());
                        fragmentShader = new Fe.Shader(Fe.ShaderType.Fragment, fragReader.ReadToEnd());
                    }
                    break;
                default:
                    throw new Exception("Unknown backend renderer type");
            }

            // Link shaders into a program for binding.
            var shaderProgram = new Fe.ShaderProgram(new Fe.Shader[] {vertexShader, fragmentShader} );                     

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

            // Create shared uniforms
            Fe.Uniform projectionUniform = new Fe.Uniform("projectionMatrix", Fe.UniformType.Matrix4x4f);

            Fe.UniformBuffer sharedUniforms = new Fe.UniformBuffer();

            // Specify number of cube dimensions
            int dim = 18;
            int totalCubeCount = dim * dim * dim;
            
            // Threshold for the timings
            const double highThreshold = 1000 / 65;
            const double lowThreshold = 1000 / 57;

            // Set a big ol command list.
            var cubeCommands = new List<Fe.Command>();
            foreach (var i in Enumerable.Range(0, 64000))
            {
                var c = new Fe.Command();
                c.ShaderProgram = shaderProgram;
                c.VertexBuffer = vb;
                c.IndexBuffer = ib;
                c.SharedUniforms = sharedUniforms;
                cubeCommands.Add(c);
            }

            // Set up a projection matrix
            var projectionMatrix = Nml.Matrix4x4.PerspectiveProjectionRH(Nml.Common.Pi / 4, (float)form.Width / (float)form.Height, 0.1f, 100.0f);            
            projectionMatrix *= Nml.Matrix4x4.Translate(-5.0f, -5.0f, -38.0f);         
            sharedUniforms.Set(projectionUniform, projectionMatrix);

            float animTime = 0.0f; // Use for animating the cubes

            // Starting point
            Nml.Vector3 initial = new Nml.Vector3(
                -0.6f * dim / 2.0f,
                -0.6f * dim / 2.0f,
                -15.0f
            );

            // Handling to switch between using threads and not.
            bool useThreading = false;
            form.KeyDown += delegate(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.F1)
                {
                    useThreading = !useThreading;
                }
            };

            // Anonymous function for creating our commandd
            Action<int, int, int> CreateCommand = (x, y, z) =>
            {
                int cubeIndex = (x * dim) + (y * dim * dim) + z;
                
                // Rotate it to make look pretty
                Nml.Quaternion rotQuat;
                Nml.Quaternion.RotateEuler(animTime + x * 0.21f, animTime + y * 0.37f, animTime + y * 0.13f, out rotQuat);
                Nml.Matrix4x4 rotMatrix;
                Nml.Quaternion.GetMatrix4x4(ref rotQuat, out rotMatrix);

                // Set translation part
                rotMatrix.M14 = initial.x + x * 0.6f;
                rotMatrix.M24 = initial.y + y * 0.6f;
                rotMatrix.M34 = initial.z + z * 0.6f;

                var c = cubeCommands[cubeIndex];                

                c.Transform = rotMatrix;
            };

            // Create some timers and run the main loop
            Stopwatch frameTime = Stopwatch.StartNew();
            Stopwatch lastUpdateTimer = Stopwatch.StartNew();
            Fe.Forms.Application.Run(form, () =>
            {
                double dt = frameTime.Elapsed.TotalMilliseconds;
                frameTime.Restart();
                
                // Increase/Decrease dimensions of cubes every so often
                if (lastUpdateTimer.Elapsed.TotalMilliseconds >= 1000)
                {
                    if (dt < highThreshold)
                    {
                        dim = dim + 2;
                    }
                    else if (dt > lowThreshold)
                    {
                        dim = Math.Max(dim - 1, 2);
                    }

                    lastUpdateTimer.Restart();

                    // Also we update title here to represent newest value.
                    form.Text = String.Format("Cube count: {0}, FrameTime: {1} , FPS: {2:N0}", totalCubeCount, dt, 1000f / dt);
                }

                totalCubeCount = dim * dim * dim;

                // Increase the animation
                animTime += (float)dt * 0.0010f;

                if (useThreading)
                {
                    // An example of a simple use of threading for the calculation of the cube commands.
                    Parallel.For(0, dim, x =>
                    {
                        for (int y = 0; y < dim; y++)
                        {
                            for (int z = 0; z < dim; z++)
                            {
                                CreateCommand(x, y, z);
                            }
                        }
                    });
                }
                else
                {
                    for (int x = 0; x < dim; x++)
                    {
                        for (int y = 0; y < dim; y++)
                        {
                            for (int z = 0; z < dim; z++)
                            {
                                CreateCommand(x, y, z);
                            }
                        }
                    }
                }

                // Submit our commands for the cubes to the renderer
                for (int i = 0; i < totalCubeCount; i++)
                {                    
                    renderer.Submit(cubeCommands[i]);  
                }

                renderer.Update();      
            });

            // Kill off the renderer and clean up all underlying resources.
            renderer.Dispose();
        }
    }
}
