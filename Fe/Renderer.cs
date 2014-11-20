using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#if RENDERER_GL
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
#endif

namespace Fe
{
    /// <summary>
    /// The Fe renderer handles all the drawing and processing of the render commands.
    /// </summary>
    public class Renderer : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Renderer"/> class.
        /// </summary>
        public Renderer()
        {
            _frameCommandBag = new Command[ushort.MaxValue];
            _nextFrameCommands = new Command[ushort.MaxValue];
            _commandCount = 0;

#if RENDERER_GL
            _glProgramCache = new ResourceCache<GLShaderProgram>(512);
            _glVBCache = new ResourceCache<GLBuffer>(4096);
            _glIBCache = new ResourceCache<GLBuffer>(4096);
#endif
            _currentState = new FrameState();
        }

        /// <summary>
        /// Initialises the renderer.
        /// </summary>
        public void Init()
        {
#if RENDERER_GL

            if (this._windowHandle != IntPtr.Zero)
            {
                // Create a GL context.
                OpenTK.Platform.IWindowInfo wi = null;
                wi = OpenTK.Platform.Utilities.CreateWindowsWindowInfo(_windowHandle);
                this._context = new OpenTK.Graphics.GraphicsContext(OpenTK.Graphics.GraphicsMode.Default, wi, 3, 2, GraphicsContextFlags.Default);
                this._context.LoadAll();
            }
            else
            {
                //TODO: Logging to say that GL context wasn't created and assumed it's been setup by someone else.
            }

            string version = GL.GetString(StringName.Version);            
            
            GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.DepthTest);
            GL.CullFace(OpenTK.Graphics.OpenGL.CullFaceMode.Back);
            GL.FrontFace(OpenTK.Graphics.OpenGL.FrontFaceDirection.Ccw);           
#endif
            // Check we've got the right version of our rendering context set up.
            CheckValidVersion();
        }

        /// <summary>
        /// Updates the renderer and advances the next frame to be rendererd.
        /// </summary>
        public void Update()
        {
            // Build the next frame            
            Array.Copy(this._frameCommandBag, this._nextFrameCommands, this._commandCount);
            
            // Draw the next frame.
            this.Draw();

            // Reset command bag
            Array.Clear(this._frameCommandBag, 0, this._commandCount);
            Array.Clear(this._nextFrameCommands, 0, this._nextFrameCommands.Length);
            this._commandCount = 0;

            this.Clean();
        }

        /// <summary>
        /// Submits the specified command to the renderer queue to be used in the next frame.
        /// This is not a thread safe method, it must be called in the same context as the Renderer was created.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Submit(Command command)
        {
            // Add the command to the bag of commands we want for the next frame.
            this._frameCommandBag[_commandCount] = command;
            // Increase total count of commands we have for next frame.
            this._commandCount++;
        }

        /// <summary>
        /// Sets the window handle to be used for creating contexts/device.
        /// </summary>
        /// <param name="handle">Pointer to a window handle.</param>
        public void SetWindowHandle(IntPtr handle)
        {
            this._windowHandle = handle;
        }

        /// <summary>
        /// Gets the type of the renderer.
        /// </summary>
        /// <returns><see cref="RendererType"/></returns>
        public RendererType GetRendererType()
        {
#if RENDERER_GL
            return RendererType.OpenGL;
#endif
        }

        /// <summary>
        /// Attempts to clean any resources that need to be.
        /// </summary>
        internal void Clean()
        {
#if RENDERER_GL
            this._glProgramCache.Clean();
            this._glIBCache.Clean();
            this._glVBCache.Clean();
#endif
        }

        /// <summary>
        /// Render a frame.
        /// </summary>
        internal void Draw()
        {
#if RENDERER_GL
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit | OpenTK.Graphics.OpenGL.ClearBufferMask.DepthBufferBit);
#endif       

            // Set the view
#if RENDERER_GL
            GL.Viewport(0, 0, 1280, 720);
#endif
            // Go through each of our commands that are for this frame.
            for (int i = 0; i < this._commandCount; i++)
            {
                // Next command to work with.
                Command command = this._nextFrameCommands[i];

                // Build Shader Program as appropriate     
#if RENDERER_GL
                GLShaderProgram program;
#endif
                // Have we already loaded and cached a shader program.
                if (command.ShaderProgram.ResourceIndex == ushort.MaxValue)
                {
#if RENDERER_GL
                    // Build a OpenGL shader program from our commands ShaderProgram data.
                    program = new GLShaderProgram(command.ShaderProgram);
                    this._glProgramCache.Add(command.ShaderProgram, program);
#endif
                }
                else
                {
#if RENDERER_GL
                    program = _glProgramCache[command.ShaderProgram.ResourceIndex];
#endif
                }                

                // Build Vertex Buffer as appropriate
#if RENDERER_GL
                GLBuffer vb;
#endif
                // Have we already loaded and cached a vertex buffer
                if (command.VertexBuffer.ResourceIndex == ushort.MaxValue)
                {
#if RENDERER_GL
                    vb = new GLBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer);
                    vb.Create(command.VertexBuffer.VertexType, command.VertexBuffer.Size, command.VertexBuffer.Data, command.VertexBuffer.Dynamic);
                    this._glVBCache.Add(command.VertexBuffer, vb);
#endif
                }
                else
                {
#if RENDERER_GL
                     vb = _glVBCache[command.VertexBuffer.ResourceIndex];
#endif
                }

                // Build Index Buffer as appropriate
#if RENDERER_GL
                GLBuffer ib;
#endif
                // Have we already loaded and cached a vertex buffer
                if (command.IndexBuffer.ResourceIndex == ushort.MaxValue)
                {
#if RENDERER_GL
                    ib = new GLBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ElementArrayBuffer);
                    ib.Create(typeof(uint), command.IndexBuffer.Size, command.IndexBuffer.Data, command.IndexBuffer.Dynamic);
                    this._glIBCache.Add(command.IndexBuffer, ib);
#endif
                }
                else
                {
#if RENDERER_GL
                    ib = _glIBCache[command.IndexBuffer.ResourceIndex];
#endif
                }

                bool uniformsChanged = false;
                // Different uniform to current state then we'll need to rebind whatever the new ones are.
                if (command.SharedUniforms != this._currentState.SharedUniforms)
                {
                    this._currentState.SharedUniforms = command.SharedUniforms;
                    uniformsChanged = true;
                }                

                // Do we need to change the state for the current active program.
                if (command.ShaderProgram != this._currentState.ShaderProgram)
                {
                    this._currentState.ShaderProgram = command.ShaderProgram;
#if RENDERER_GL

                    GL.UseProgram(program.ProgramRef);

                    // Bind the vertex buffer
                    if (vb != null)
                    {
                        GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, vb.BufferId);

                        uint index = 0;

                        // Bind attributes that are defined for the vertex buffer
                        var type = Activator.CreateInstance(command.VertexBuffer.VertexType) as IVertex;
                        foreach (var attribute in type.VertexDeclaration.Attributes)
                        {
                            VertexAttribPointerType glAttribType;
                            glAttribMapping.TryGetValue(attribute.Type, out glAttribType);

                            GL.EnableVertexAttribArray(index);
                            GL.VertexAttribPointer(index, attribute.Size, glAttribType, attribute.Normalised, type.VertexDeclaration.Stride, new IntPtr(type.VertexDeclaration.Offsets[attribute.Name]));

                            index += 1;
                        }
                    }

                    // Bind the Index Buffer
                    if (ib != null)
                    {
                        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ib.BufferId);
                    }

                    // Program changed, so we'll need to change the uniforms.
                    uniformsChanged = true;                    
#endif
                }

                // If we have a uniform buffer in the command and we've said we need to change them, then rebind them all to the current program.
                if (command.SharedUniforms != null && uniformsChanged)
                {
                    int uniformLocation;
                    foreach (var uniform in command.SharedUniforms._uniforms)
                    {                        
                        if (program.Uniforms.TryGetValue(uniform.Key.Name, out uniformLocation))
                        {
                            if (uniform.Key.Type == UniformType.Matrix4x4f)
                            {
                                var matrix = (Nml.Matrix4x4)uniform.Value;
                                unsafe
                                {
                                    float* matrix_ptr = &matrix.M11;
                                    {
                                        GL.UniformMatrix4(uniformLocation, 1, false, matrix_ptr);
                                    }
                                }
                            }
                            else if (uniform.Key.Type == UniformType.Uniform1f)
                            {
                                GL.Uniform1(uniformLocation, (float)uniform.Value);
                            }
                            else if (uniform.Key.Type == UniformType.Uniform2f)
                            {
                                float[] value = uniform.Value as float[];
                                GL.Uniform2(uniformLocation, value[0], value[1]);
                            }
                            else if (uniform.Key.Type == UniformType.Uniform3f)
                            {
                                float[] value = uniform.Value as float[];
                                GL.Uniform3(uniformLocation, value[0], value[1], value[2]);
                            }
                            else if (uniform.Key.Type == UniformType.Uniform4f)
                            {
                                float[] value = uniform.Value as float[];
                                GL.Uniform4(uniformLocation, value[0], value[1], value[2], value[3]);
                            }
                        }                        
                    }

                    // Predefined locations
                    if (program.Uniforms.TryGetValue("_model", out uniformLocation))
                    {
                        predefinedModelUniformLocation = uniformLocation;
                    }
                }

                // Per command transform
                if (command.Transform != null && predefinedModelUniformLocation != -1)
                {                   
                    Nml.Matrix4x4 matrix = command.Transform;

                    unsafe
                    {
                        float* matrix_ptr = &matrix.M11;
                        {
                            GL.UniformMatrix4(predefinedModelUniformLocation, 1, false, matrix_ptr);
                        }
                    }                    
                }

                // Lets draw!
                GL.DrawElements(OpenTK.Graphics.OpenGL.PrimitiveType.Triangles, ib.Size, OpenTK.Graphics.OpenGL.DrawElementsType.UnsignedInt, IntPtr.Zero);

            }  // End of command list

#if RENDERER_GL            
            _context.SwapBuffers();
#endif
        }

        [DllImport("gdi32")]
        private static extern int SwapBuffers(IntPtr hdc);


        /// <summary>
        /// Checks the version to ensure we have a valid rendering context.
        /// </summary>
        private void CheckValidVersion()
        {
#if RENDERER_GL
            int major, minor;
            unsafe
            {
                major = GL.GetInteger(GetPName.MajorVersion);
                minor = GL.GetInteger(GetPName.MinorVersion);
            }

            if (major >= 3)
            {
                // Nothing lower than GL 3.2
                if (major == 3 && minor < 2)
                {
                    throw new Exception("OpenGL context version must be higher than 3.2");
                }
            }
#endif
        }

        private IntPtr _windowHandle; // Window handle.

        private Command[] _nextFrameCommands; // Sorted list of commands that will be used for the next frame.
        private Command[] _frameCommandBag; // Commands submitted for the next frame.
        private int _commandCount = 0; // Number of commands added for the next frame.        

        private FrameState _currentState;

#if RENDERER_GL
        private ResourceCache<GLShaderProgram> _glProgramCache;
        private ResourceCache<GLBuffer> _glVBCache;
        private ResourceCache<GLBuffer> _glIBCache;
        private IGraphicsContext _context;
        //private GLContext _context;
        private int predefinedModelUniformLocation;

        /// <summary>
        /// The gl attribute mapping
        /// </summary>
        private static Dictionary<VertexAttributeType, OpenTK.Graphics.OpenGL.VertexAttribPointerType> glAttribMapping = new Dictionary<VertexAttributeType, OpenTK.Graphics.OpenGL.VertexAttribPointerType>() 
        {
            {VertexAttributeType.Float, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float}, 
            {VertexAttributeType.Byte, OpenTK.Graphics.OpenGL.VertexAttribPointerType.UnsignedByte}, 
            {VertexAttributeType.Short, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Short}
        };

#endif

        #region Cleanup

        private void Destroy()
        {
#if RENDERER_GL
            this._glProgramCache.Clean(true);
            this._glIBCache.Clean(true);
            this._glVBCache.Clean(true);
            _context.Dispose();            
#endif
        }

        private bool _disposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// This must be called in order to get the correct cleanup before things like the context is destroyed.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.                    
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                this.Destroy();

                // Note disposing has been done.
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Renderer"/> class.
        /// </summary>
        ~Renderer()
        {
            Dispose(false);
        }

        #endregion
    }
}
