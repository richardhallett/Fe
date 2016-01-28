﻿using System;
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
            _nextFrameCommands = new Command[MaxCommands];
            
            _commandCount = 0;
            _views = new Dictionary<byte, View>();

#if RENDERER_GL
            _glProgramCache = new ResourceCache<GLShaderProgram>(MaxShaderPrograms);
            _glVBCache = new ResourceCache<GLBuffer>(MaxVertexBuffers);
            _glIBCache = new ResourceCache<GLBuffer>(MaxIndexBuffers);
#endif
            _currentState = new FrameState();

            _defaultBlendState = new BlendState();
            _defaultDepthState = new DepthState();
            _defaultRasteriserState = new RasteriserState();
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

        /// <summary>
        /// Main rendering code
        /// </summary>
        public void RenderThread()
        {
#if RENDERER_GL
            this._context.MakeCurrent(this._windowInfo);            
                       
            // Create a dummy VAO as it's required for Core profile
            var vaos = new int[1];
            GL.GenVertexArrays(1, vaos);
            GL.BindVertexArray(vaos[0]);
#endif
            // Check we've got the right version of our rendering context set up.
            CheckValidVersion();

            // Do an initial reset.
            Reset(this.Width, this.Height);

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

        ///// <summary>
        ///// Creates a shader program.        
        ///// </summary>
        ///// <param name="shaders">The shaders.</param>
        ///// <returns>A ShaderProgram that has been referenced internally by the renderer. It is the responsibility of the the user to hold onto the ShaderProgram or it will be cleaned up.</returns>
        //public ShaderProgram CreateShaderProgram(IReadOnlyList<Shader> shaders)
        //{
        //    var shaderProgram = new ShaderProgram(shaders);
        //    this._glProgramCache.Add(shaderProgram);
        //    return shaderProgram;
        //}        

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

                bool viewChanged = false;
                View view = null;
                if (command.ViewId != this._currentState.ViewId)
                {
                    viewChanged = true;
                    this._currentState.ViewId = command.ViewId;
                    
                    if (!_views.TryGetValue(command.ViewId, out view))
                    {
                        //TODO: Logging to say we failed loading a command specific view
                        view = _defaultView;
                    }
                    
                    ResetViewPort(view);
                }

                if (command.ShaderProgram == null)
                {
                    //TODO: Log the fact it skipped a command due to no shader program
                    continue;
                }

                // Build Shader Program as appropriate     
                GLShaderProgram program;
                // Have we already loaded and cached a shader program.
                if (!command.ShaderProgram.Created)
                {
                    // Build a OpenGL shader program from our commands ShaderProgram data.
                    program = new GLShaderProgram(command.ShaderProgram);
                    this._glProgramCache.Add(command.ShaderProgram, program);
                }
                else
                {
                    program = this._glProgramCache[command.ShaderProgram.ResourceIndex];
                }              

                // Set default blend state for anything that doesn't have one explicitly set.
                if(command.BlendState == null)
                {
                    command.BlendState = _defaultBlendState;
                }

                // Is the blend state different if so we need to set it.
                if (command.BlendState != this._currentState.BlendState)
                {
                    this._currentState.BlendState = command.BlendState;
                    var bs = command.BlendState;
                    
                    if (bs.EnableBlending)
                    {
                        GL.Enable(EnableCap.Blend);

                        var colourOp = glBlendOperationMapping[bs.ColourOperation];
                        var alphaOp = glBlendOperationMapping[bs.AlphaOperation];

                        GL.BlendEquationSeparate(colourOp, alphaOp);

                        var sourceColour = glSrcBlendFactorMapping[bs.SourceBlendColour];
                        var destColour = glDstBlendFactorMapping[bs.DestinationBlendColour];
                        var sourceAlpha = glSrcBlendFactorMapping[bs.SourceBlendAlpha];
                        var destAlpha = glDstBlendFactorMapping[bs.SourceBlendAlpha];

                        GL.BlendFuncSeparate(sourceColour, destColour, sourceAlpha, destAlpha);

                        if(bs.SourceBlendColour == BlendFactor.ConstantColour | bs.DestinationBlendColour == BlendFactor.ConstantColour && bs.BlendConstant != this._currentState.BlendState.BlendConstant)
                        {
                            GL.BlendColor(bs.BlendConstant.Red, bs.BlendConstant.Green,
                                          bs.BlendConstant.Blue, bs.BlendConstant.Alpha);
                        }
                    } else
                    {
                        GL.Disable(EnableCap.Blend);
                    }
                }

                // Set default depth state for anything that doesn't have one explicitly set.
                if (command.DepthState == null)
                {
                    command.DepthState = _defaultDepthState;
                }

                // Is the depth state different if so we need to set it.
                if (command.DepthState != this._currentState.DepthState)
                {
                    this._currentState.DepthState = command.DepthState;
                    var ds = command.DepthState;

                    // Depth Test
                    if (ds.EnableDepthTest)
                    {
                        GL.Enable(EnableCap.DepthTest);
                        var df = glDepthFuncMapping[ds.DepthFunc];

                        GL.DepthFunc(df);
                    }
                    else
                    {
                        GL.Disable(EnableCap.DepthTest);
                    }

                    // Depth write
                    if (ds.EnableDepthWrite)
                    {
                        GL.DepthMask(true);
                    }
                    else
                    {
                        GL.DepthMask(false);
                    }
                }

                // Set default rasteriser state for anything that doesn't have one explicitly set.
                if (command.RasteriserState == null)
                {
                    command.RasteriserState = _defaultRasteriserState;
                }

                // Is the rasteriser state different if so we need to set it.
                if (command.RasteriserState != this._currentState.RasteriserState)
                {
                    this._currentState.RasteriserState = command.RasteriserState;
                    var rs = command.RasteriserState;

                    // Cull mode
                    if (rs.CullMode == CullMode.Clockwise)
                    {
                        GL.Enable(EnableCap.CullFace);
                        GL.CullFace(OpenTK.Graphics.OpenGL.CullFaceMode.Back);
                    }
                    else if (rs.CullMode == CullMode.CounterClockwise)
                    {
                        GL.Enable(EnableCap.CullFace);
                        GL.CullFace(OpenTK.Graphics.OpenGL.CullFaceMode.Front);
                    }
                    else
                    {
                        GL.Disable(EnableCap.CullFace);
                    }

                    // Multisampling
                    if (rs.EnableMultisampling)
                    {
                        GL.Enable(EnableCap.Multisample);
                    }
                    else
                    {
                        GL.Disable(EnableCap.Multisample);
                    }
                }

                bool sharedUniformsChanged = false;
                // Different uniform to current state then we'll need to rebind whatever the new ones are.
                if (command.SharedUniforms != null && command.SharedUniforms != this._currentState.SharedUniforms)
                {
                    this._currentState.SharedUniforms = command.SharedUniforms;
                    sharedUniformsChanged = true;
                }

                // Do we need to change the state for the current active program.
                bool programChanged = false;
                if (command.ShaderProgram != this._currentState.ShaderProgram)
                {
                    this._currentState.ShaderProgram = command.ShaderProgram;

                    GL.UseProgram(program.ProgramRef);                  

                    // Predefined locations
                    int uniformLocation;
                    // Model transform
                    if (program.Uniforms.TryGetValue("_model", out uniformLocation))
                    {
                        this.predefinedModelUniformLocation = uniformLocation;
                    }

                    // View transform
                    if (program.Uniforms.TryGetValue("_view", out uniformLocation))
                    {
                        this.predefinedViewUniformLocation = uniformLocation;                        
                    }

                    // Projection transform
                    if (program.Uniforms.TryGetValue("_projection", out uniformLocation))
                    {
                        this.predefinedProjectionUniformLocation = uniformLocation;                        
                    }

                    // Program changed, so we'll need to change the uniforms.
                    sharedUniformsChanged = true;
                    programChanged = true;
                }

                // Build Vertex Buffer as appropriate
                GLBuffer vb = null;
                if (command.VertexBuffer != null)
                {
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
                }

                // Build Index Buffer as appropriate
                GLBuffer ib = null;
                if (command.IndexBuffer != null)
                {
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
                }

                if (programChanged || command.VertexBuffer != _currentState.VertexBuffer || command.IndexBuffer != _currentState.IndexBuffer)
                {
                    _currentState.VertexBuffer = command.VertexBuffer;
                    _currentState.IndexBuffer = command.IndexBuffer;

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
                }

                // For view changes we need to set view/projection matrices again
                if (viewChanged)
                {
                    // View transform
                    if (predefinedViewUniformLocation != -1)
                    {
                        Nml.Matrix4x4 viewMatrix = Nml.Matrix4x4.Identity;
                        viewMatrix = view.ViewMatrix;

                        unsafe
                        {
                            float* matrix_ptr = &viewMatrix.M11;
                            {
                                GL.UniformMatrix4(predefinedViewUniformLocation, 1, false, matrix_ptr);
                            }
                        }
                    }

                    // Projection transform
                    if (predefinedProjectionUniformLocation != -1)
                    {
                        Nml.Matrix4x4 projectionMatrix = Nml.Matrix4x4.Identity;
                        projectionMatrix = view.ProjectionMatrix;

                        unsafe
                        {
                            float* matrix_ptr = &projectionMatrix.M11;
                            {
                                GL.UniformMatrix4(predefinedProjectionUniformLocation, 1, false, matrix_ptr);
                            }
                        }
                    }
                }

                // If we have a uniform buffer in the command and we've said we need to change them, then rebind them all to the current program.
                if (command.SharedUniforms != null && sharedUniformsChanged)
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
                }

                // Per command transform                
                if (this.predefinedModelUniformLocation != -1 && command.Transform != null)
                {
                    Nml.Matrix4x4 transformMatrix = Nml.Matrix4x4.Identity;
                    transformMatrix = command.Transform.Value;

                    unsafe
                    {
                        float* matrix_ptr = &transformMatrix.M11;
                        {
                            GL.UniformMatrix4(this.predefinedModelUniformLocation, 1, false, matrix_ptr);
                        }
                    }
                }

                // Work out what primitive topology we should be using
                OpenTK.Graphics.OpenGL.PrimitiveType primType = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
                if (command.PrimitiveType != _currentState.PrimitiveType)
                {
                    _currentState.PrimitiveType = command.PrimitiveType;
                    primType = glPrimitiveTypeMapping[command.PrimitiveType];
                }

                // Lets draw!
                if (ib != null)
                {
                    GL.DrawElements(primType, ib.Size, OpenTK.Graphics.OpenGL.DrawElementsType.UnsignedInt, IntPtr.Zero);
                }
                else
                {
                    GL.DrawArrays(primType, 0, vb.Size);
                }
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

        // Constants
        private const int MaxShaderPrograms = 512;
        private const int MaxVertexBuffers = 4096;
        private const int MaxIndexBuffers = 4096;
        private const int MaxCommands = 131070;

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
        
        private View _defaultView; // Default view when none have been sent
        private Dictionary<byte, View> _views; // Stored views

        private BlendState _defaultBlendState; // Default blend state when none has been set
        private DepthState _defaultDepthState; // Default depth state when none has been set
        private RasteriserState _defaultRasteriserState; // Default rasterisation state when none has been set

        // Holds current state of the frame
        internal FrameState _currentState;

#if RENDERER_GL
        internal ResourceCache<GLShaderProgram> _glProgramCache;
        internal ResourceCache<GLBuffer> _glVBCache;
        internal ResourceCache<GLBuffer> _glIBCache;
        private IGraphicsContext _context;        
        internal int predefinedModelUniformLocation = -1;
        internal int predefinedViewUniformLocation = -1;
        internal int predefinedProjectionUniformLocation = -1;

        /// <summary>
        /// The gl attribute mapping
        /// </summary>
        internal static Dictionary<VertexAttributeType, OpenTK.Graphics.OpenGL.VertexAttribPointerType> glAttribMapping = new Dictionary<VertexAttributeType, OpenTK.Graphics.OpenGL.VertexAttribPointerType>() 
        {
            {VertexAttributeType.Float, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float}, 
            {VertexAttributeType.Byte, OpenTK.Graphics.OpenGL.VertexAttribPointerType.UnsignedByte}, 
            {VertexAttributeType.Short, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Short}
        };

        /// <summary>
        /// Mapping of BlendFactor to OpenGL equivelents.
        /// </summary>
        internal static Dictionary<BlendFactor, BlendingFactorSrc> glSrcBlendFactorMapping = new Dictionary<BlendFactor, BlendingFactorSrc>()
        {
            {BlendFactor.Zero, BlendingFactorSrc.Zero},
            {BlendFactor.One, BlendingFactorSrc.One},
            {BlendFactor.SourceColour, BlendingFactorSrc.SrcColor},
            {BlendFactor.InvertSourceColour, BlendingFactorSrc.OneMinusSrcColor},
            {BlendFactor.DestinationColour, BlendingFactorSrc.DstColor},
            {BlendFactor.InvertDestinationColour, BlendingFactorSrc.OneMinusDstColor},
            {BlendFactor.SourceAlpha, BlendingFactorSrc.SrcAlpha},
            {BlendFactor.InvertSourceAlpha, BlendingFactorSrc.OneMinusSrcAlpha},
            {BlendFactor.DestinationAlpha, BlendingFactorSrc.DstAlpha},
            {BlendFactor.InvertDestinationAlpha, BlendingFactorSrc.OneMinusDstAlpha},
            {BlendFactor.ConstantColour, BlendingFactorSrc.ConstantColor},
            {BlendFactor.InvertConstantColour, BlendingFactorSrc.OneMinusConstantColor},
            {BlendFactor.ConstantAlpha, BlendingFactorSrc.ConstantAlpha},
            {BlendFactor.InvertConstantAlpha, BlendingFactorSrc.OneMinusConstantAlpha},
            {BlendFactor.SourceAlphaSaturation, BlendingFactorSrc.SrcAlphaSaturate},
            {BlendFactor.Source1Colour, BlendingFactorSrc.Src1Color},
            {BlendFactor.InvertSource1Colour, BlendingFactorSrc.OneMinusSrc1Color},
            {BlendFactor.Source1Alpha, BlendingFactorSrc.Src1Alpha},
            {BlendFactor.InvertSource1Alpha, BlendingFactorSrc.OneMinusSrc1Alpha}
        };

        /// <summary>
        /// Mapping of BlendFactor to OpenGL equivelents.
        /// </summary>
        internal static Dictionary<BlendFactor, BlendingFactorDest> glDstBlendFactorMapping = new Dictionary<BlendFactor, BlendingFactorDest>()
        {
            {BlendFactor.Zero, BlendingFactorDest.Zero},
            {BlendFactor.One, BlendingFactorDest.One},
            {BlendFactor.SourceColour, BlendingFactorDest.SrcColor},
            {BlendFactor.InvertSourceColour, BlendingFactorDest.OneMinusSrcColor},
            {BlendFactor.DestinationColour, BlendingFactorDest.DstColor},
            {BlendFactor.InvertDestinationColour, BlendingFactorDest.OneMinusDstColor},
            {BlendFactor.SourceAlpha, BlendingFactorDest.SrcAlpha},
            {BlendFactor.InvertSourceAlpha, BlendingFactorDest.OneMinusSrcAlpha},
            {BlendFactor.DestinationAlpha, BlendingFactorDest.DstAlpha},
            {BlendFactor.InvertDestinationAlpha, BlendingFactorDest.OneMinusDstAlpha},
            {BlendFactor.ConstantColour, BlendingFactorDest.ConstantColor},
            {BlendFactor.InvertConstantColour, BlendingFactorDest.OneMinusConstantColor},
            {BlendFactor.ConstantAlpha, BlendingFactorDest.ConstantAlpha},
            {BlendFactor.InvertConstantAlpha, BlendingFactorDest.OneMinusConstantAlpha},
            {BlendFactor.SourceAlphaSaturation, BlendingFactorDest.SrcAlphaSaturate},
            {BlendFactor.Source1Colour, BlendingFactorDest.Src1Color},
            {BlendFactor.InvertSource1Colour, BlendingFactorDest.OneMinusSrc1Color},
            {BlendFactor.Source1Alpha, BlendingFactorDest.Src1Alpha},
            {BlendFactor.InvertSource1Alpha, BlendingFactorDest.OneMinusSrc1Alpha}
        };

        internal static Dictionary<BlendOperation, BlendEquationMode> glBlendOperationMapping = new Dictionary<BlendOperation, BlendEquationMode>()
        {
            {BlendOperation.Add, BlendEquationMode.FuncAdd},
            {BlendOperation.Subtract, BlendEquationMode.FuncSubtract},
            {BlendOperation.ReverseSubtract, BlendEquationMode.FuncReverseSubtract},
            {BlendOperation.Max, BlendEquationMode.Max},
            {BlendOperation.Min, BlendEquationMode.Min}
        };

        internal static Dictionary<DepthFunc, DepthFunction> glDepthFuncMapping = new Dictionary<DepthFunc, DepthFunction>()
        {
            {DepthFunc.Never, DepthFunction.Never},
            {DepthFunc.Less, DepthFunction.Less},
            {DepthFunc.Equal, DepthFunction.Equal},
            {DepthFunc.LessEqual, DepthFunction.Lequal},
            {DepthFunc.Greater, DepthFunction.Greater},
            {DepthFunc.NotEqual, DepthFunction.Notequal},
            {DepthFunc.GreaterEqual, DepthFunction.Gequal},
            {DepthFunc.Always, DepthFunction.Always}
        };

        internal static Dictionary<PrimitiveType, OpenTK.Graphics.OpenGL.PrimitiveType> glPrimitiveTypeMapping = new Dictionary<PrimitiveType, OpenTK.Graphics.OpenGL.PrimitiveType>()
        {
            {PrimitiveType.Triangles, OpenTK.Graphics.OpenGL.PrimitiveType.Triangles},
            {PrimitiveType.TriangleStrip, OpenTK.Graphics.OpenGL.PrimitiveType.TriangleStrip},
            {PrimitiveType.Lines, OpenTK.Graphics.OpenGL.PrimitiveType.Lines},
            {PrimitiveType.LineStrip, OpenTK.Graphics.OpenGL.PrimitiveType.LineStrip},
            {PrimitiveType.Points, OpenTK.Graphics.OpenGL.PrimitiveType.Points}
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
