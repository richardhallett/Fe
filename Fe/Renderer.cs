using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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
            _commandBuckets = new List<CommandBucket>();
            _nextFrameCommands = new Command[ushort.MaxValue];
            
            _commandCount = 0;
            _views = new Dictionary<byte, View>();

#if RENDERER_GL
            _glProgramCache = new ResourceCache<GLShaderProgram>(512);
            _glVBCache = new ResourceCache<GLBuffer>(4096);
            _glIBCache = new ResourceCache<GLBuffer>(4096);
#endif
            _currentState = new FrameState();            
        }

        /// <summary>
        /// Width of the renderers surface.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height of the renderers surface.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Initialises the this.
        /// </summary>
        public void Init()
        {
            if (this._windowHandle != IntPtr.Zero)
            {
                // Create a GL context.
                this._windowInfo = OpenTK.Platform.Utilities.CreateWindowsWindowInfo(_windowHandle);
                this._context = new OpenTK.Graphics.GraphicsContext(OpenTK.Graphics.GraphicsMode.Default, this._windowInfo, 3, 2, GraphicsContextFlags.Default);
                this._context.LoadAll();
                this._context.MakeCurrent(null);                
            }
            else
            {
                //TODO: Logging to say that GL context wasn't created and assumed it's been setup by someone else.
            }

            _renderThread = new Thread(this.RenderThread);
            _renderThread.IsBackground = true;
            _renderThread.Start();
        }

        public void RenderThread()
        {
#if RENDERER_GL
            this._context.MakeCurrent(this._windowInfo);

            GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.DepthTest);
            GL.CullFace(OpenTK.Graphics.OpenGL.CullFaceMode.Back);
            GL.FrontFace(OpenTK.Graphics.OpenGL.FrontFaceDirection.Ccw);

            // Create a dummy VAO as it's required for Core profile
            var vaos = new int[1];
            GL.GenVertexArrays(1, vaos);
            GL.BindVertexArray(vaos[0]);
#endif
            // Check we've got the right version of our rendering context set up.
            CheckValidVersion();

            while (!_stopRendering)
            {                                
                this._canRenderEvent.WaitOne();

                // Draw the next frame.                               
                this.Draw();

                // Reset command bag
                this._commandCount = 0;

                // Clean resources
                this.Clean();

                // Release lock to allow updates to come in.
                this._updateSem.Release();
            }
            
            this.Destroy();            
        }

        /// <summary>
        /// Updates the renderer and advances the next frame to be rendererd.
        /// </summary>
        public void EndFrame()
        {
            this._canRenderEvent.Set();
            this._updateSem.Wait();

            foreach (var bucket in _commandBuckets)
            {
                bucket.Sort();

                this._commandCount += bucket.Submit(ref this._nextFrameCommands, this._commandCount);
            }           
        }

        /// <summary>
        /// Adds a command bucket into the internal list to be used when submitting commands to the renderer.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="viewId">The view identifier.</param>
        /// <returns></returns>
        public CommandBucket AddCommandBucket(uint size, byte viewId = 0)
        {
            var bucket = new CommandBucket(size, viewId);
            _commandBuckets.Add(bucket);

            return bucket;
        }

        /// <summary>
        /// Creates a shader program.        
        /// </summary>
        /// <param name="shaders">The shaders.</param>
        /// <returns>A ShaderProgram that has been referenced internally by the renderer. It is the responsibility of the the user to hold onto the ShaderProgram or it will be cleaned up.</returns>
        public ShaderProgram CreateShaderProgram(IReadOnlyList<Shader> shaders)
        {
            var shaderProgram = new ShaderProgram(shaders);
            this._glProgramCache.Add(shaderProgram);
            return shaderProgram;
        }

        /// <summary>
        /// Submits the specified command to the renderer queue to be used in the next frame.
        /// This is not a thread safe method, it must be called in the same context as the Renderer was created.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="viewId">The id of the view which the renderer will execute this command</param>
//        public void Submit(CommandOld command, byte viewId = 0)
//        {
//            if (_commandCount >= ushort.MaxValue)
//            {
//                //TODO: Debug logging.
//                return; // Oops can't add a command when we have more than we support.
//            }

//            Command newCommand = new Command();
//            newCommand.IndexBuffer = command.IndexBuffer;
//            newCommand.VertexBuffer = command.VertexBuffer;
//            newCommand.SharedUniforms = command.SharedUniforms;
//            newCommand.ShaderProgram = command.ShaderProgram;

//            // Have we already loaded and cached a shader program.
//            if (command.ShaderProgram.ResourceIndex == ushort.MaxValue)
//            {
//#if RENDERER_GL
//                // Build a OpenGL shader program from our commands ShaderProgram data.
//                GLShaderProgram program = new GLShaderProgram(command.ShaderProgram);
//                this._glProgramCache.Add(command.ShaderProgram, program);
//#endif
//            }

//            newCommand.viewId = viewId;
//            //newCommand.sortKey = ulong.MaxValue;

//            newCommand.TransformMatrixIndex = -1;
 
//            if (command.Transform != null)
//            {
//                if (_matrixCacheCount >= ushort.MaxValue)
//                {
//                    //TODO: Debug logging.
//                    return; // Oops can't add this command because we have nowhere to store it's matrix.
//                }
//                this._matrixCache[_matrixCacheCount] = command.Transform.Value;
//                newCommand.TransformMatrixIndex = _matrixCacheCount;
//                _matrixCacheCount++;
//            }

//            // Work out the sort key
//            //ulong sortKey = (ulong)newCommand.ShaderProgram.ResourceIndex << 56 | (ulong)newCommand.viewId << 40;
//            //ulong sortKey = (ulong)newCommand.ShaderProgram.ResourceIndex << 56 | (ulong)newCommand.viewId << 40 | 32 << 8;

//            ulong depth = 0;
//            ulong programkey = (ulong)newCommand.ShaderProgram.ResourceIndex << 0x20;
//            ulong trans = 0 << 0x29;
//            ulong seq = 0 << 0x2c;
//            ulong view = (ulong)newCommand.viewId << 0x37;
//            ulong sortKey = depth | programkey | trans | (ulong)1<<0x2b | seq | view;
            
//            this._sortKeys[_commandCount] = sortKey;

//            // Add the command to the bag of commands we want for the next frame.
//            this._frameCommandBag[_commandCount] = newCommand;            

//            // Increase total count of commands we have for next frame.
//            this._commandCount++;
//        }

        /// <summary>
        /// Stores a view against a given identifier.
        /// </summary>
        /// <param name="viewId">The view identifier.</param>
        /// <param name="view">The view.</param>
        public void SetView(byte viewId, View view)
        {
            this._views[viewId] = view;
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
        /// Gets the type of the this.
        /// </summary>
        /// <returns><see cref="RendererType"/></returns>
        public RendererType GetRendererType()
        {
#if RENDERER_GL
            return RendererType.OpenGL;
#endif
        }

        /// <summary>
        /// Reset the this.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void Reset(int width, int height)
        {
            // Invalidate the current state
            this._currentState = new FrameState();

            // Store size details
            this.Width = width;
            this.Height = height;

            // Reset the default view
            this._defaultView = new View(0, 0, width, height);
            this._defaultView.ClearColour = new Colour4(0x719AB7FF);

            ResetViewPort(this._defaultView);
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

        internal void ResetViewPort(View view)
        {
#if RENDERER_GL
            GL.Viewport(view.X, view.Y, view.Width, view.Height);

            // If we've defined a rectangle to scissor with for this view then lets do it.
            if (view.ScissorRect != null)
            {
                GL.Enable(EnableCap.ScissorTest);
                GL.Scissor(view.ScissorRect.Value.X, view.ScissorRect.Value.Y, view.ScissorRect.Value.Width, view.ScissorRect.Value.Height);
            }

            GL.ClearColor(view.ClearColour.Red, view.ClearColour.Green, view.ClearColour.Blue, view.ClearColour.Alpha);
            GL.ClearDepth(view.ClearDepth);
            GL.Clear(OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit | OpenTK.Graphics.OpenGL.ClearBufferMask.DepthBufferBit);
#endif
        }

        /// <summary>
        /// Render a frame.
        /// </summary>
        internal void Draw()
        {
#if RENDERER_GL
            GL.Clear(OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit | OpenTK.Graphics.OpenGL.ClearBufferMask.DepthBufferBit);
#endif
            // Go through each of our commands that are for this frame.
            for (int i = 0; i < this._commandCount; i++)
            {
                // Next command to work with.
                Command command = this._nextFrameCommands[i];

#if RENDERER_GL
                
                if (command.ViewId != this._currentState.ViewId)
                {
                    this._currentState.ViewId = command.ViewId;

                    View view;
                    if (!_views.TryGetValue(command.ViewId, out view))
                    {
                        //TODO: Logging to say we failed loading a command specific view
                        view = _defaultView;
                    }
                    
                    ResetViewPort(view);
                }

                // Build Shader Program as appropriate     
                GLShaderProgram program;
                // Have we already loaded and cached a shader program.
                if (!command.ShaderProgram.Created)
                {
                    // Build a OpenGL shader program from our commands ShaderProgram data.
                    program = new GLShaderProgram(command.ShaderProgram);
                    this._glProgramCache.SetResource(command.ShaderProgram, program);
                }
                else
                {
                    program = this._glProgramCache[command.ShaderProgram.ResourceIndex];
                }

                // Build Vertex Buffer as appropriate
                GLBuffer vb;
                // Have we already loaded and cached a vertex buffer
                if (!command.VertexBuffer.Created)
                {
                    vb = new GLBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer);
                    vb.Create(command.VertexBuffer.VertexType, command.VertexBuffer.Size, command.VertexBuffer.Data, command.VertexBuffer.Dynamic);
                    this._glVBCache.Add(command.VertexBuffer, vb);
                }
                else
                {
                    vb = this._glVBCache[command.VertexBuffer.ResourceIndex];
                }

                // Do we need to update the vertex buffer data
                if (command.VertexBuffer.Changed)
                {
                    vb.Update(command.VertexBuffer.Data, 0);
                    command.VertexBuffer.Changed = false;
                }

                // Build Index Buffer as appropriate
                GLBuffer ib;

                // Have we already loaded and cached a vertex buffer
                if (!command.IndexBuffer.Created)
                {
                    ib = new GLBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ElementArrayBuffer);
                    ib.Create(typeof(uint), command.IndexBuffer.Size, command.IndexBuffer.Data, command.IndexBuffer.Dynamic);
                    this._glIBCache.Add(command.IndexBuffer, ib);
                }
                else
                {
                    ib = this._glIBCache[command.IndexBuffer.ResourceIndex];
                }

                // Do we need to update the index buffer data
                if (command.IndexBuffer.Changed)
                {
                    ib.Update(command.IndexBuffer.Data, 0);
                    command.IndexBuffer.Changed = false;
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
                            Renderer.glAttribMapping.TryGetValue(attribute.Type, out glAttribType);

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
                        this.predefinedModelUniformLocation = uniformLocation;
                    }
                }

                // Per command transform
                Nml.Matrix4x4 transformMatrix = Nml.Matrix4x4.Identity;
                if (this.predefinedModelUniformLocation != -1)
                {
                    transformMatrix = command.Transform;

                    unsafe
                    {
                        float* matrix_ptr = &transformMatrix.M11;
                        {
                            GL.UniformMatrix4(this.predefinedModelUniformLocation, 1, false, matrix_ptr);
                        }
                    }
                }

                // Lets draw!
                GL.DrawElements(OpenTK.Graphics.OpenGL.PrimitiveType.Triangles, ib.Size, OpenTK.Graphics.OpenGL.DrawElementsType.UnsignedInt, IntPtr.Zero);
#endif

            }  // End of command list

#if RENDERER_GL    
            _context.SwapBuffers();
#endif
        }

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
        private OpenTK.Platform.IWindowInfo _windowInfo;

        private List<CommandBucket> _commandBuckets; // Sortable buckets of commands        
        private Command[] _nextFrameCommands; // Sorted list of commands that will be used for the next frame.
        private int _commandCount = 0; // Number of commands added for the next frame.        

        // For handling the render update thread access
        private readonly AutoResetEvent _canRenderEvent = new AutoResetEvent(false);
        private readonly SemaphoreSlim _updateSem = new SemaphoreSlim(0, 2);
        private volatile bool _stopRendering = false;
        private Thread _renderThread;

        internal FrameState _currentState;

        private View _defaultView; // Default view when none have been sent
        private Dictionary<byte, View> _views; // Stored views

#if RENDERER_GL
        internal ResourceCache<GLShaderProgram> _glProgramCache;
        internal ResourceCache<GLBuffer> _glVBCache;
        internal ResourceCache<GLBuffer> _glIBCache;
        private IGraphicsContext _context;        
        internal int predefinedModelUniformLocation;

        /// <summary>
        /// The gl attribute mapping
        /// </summary>
        internal static Dictionary<VertexAttributeType, OpenTK.Graphics.OpenGL.VertexAttribPointerType> glAttribMapping = new Dictionary<VertexAttributeType, OpenTK.Graphics.OpenGL.VertexAttribPointerType>() 
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

            this._context.MakeCurrent(null);
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

                this._canRenderEvent.Set();
                this._updateSem.Release();
                this._stopRendering = true;
                // Wait for the renderer thread to clean up.
                _renderThread.Join();

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.                

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
