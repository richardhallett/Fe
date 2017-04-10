using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Fe
{
    /// <summary>
    /// Represents an internal OpenGL Texture
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal class GLTexture : IDisposable
    {
        /// <summary>
        /// Creates the specified texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        public void Create(Texture texture)
        {            
            switch (texture.TextureType)
            {
                case TextureType.Texture2D:
                    TextureTarget = TextureTarget.Texture2D;
                    break;
            }            

            TextureRef = GL.GenTexture();

            Build(texture);
        }

        /// <summary>
        /// Builds a texture
        /// </summary>
        /// <param name="TextureBase">The texture data.</param>
        public void Build(Texture texture)
        {
            GL.BindTexture(TextureTarget, TextureRef);

            // Figure out our texture format data and throw an exception if we can't work it out.
            (var pif, var pf, var pt) = MapTextureFormat(texture.SampleFormat);

            var data_ptr = GCHandle.Alloc(texture.Data, GCHandleType.Pinned);
            try
            {
                // Tell opengl about our texture
                GL.TexImage2D(TextureTarget, 0, pif, texture.Width, texture.Height, 0, pf, pt, data_ptr.AddrOfPinnedObject());
            }
            finally
            {
                data_ptr.Free();
            }

            // Stop working with this texture
            GL.BindTexture(TextureTarget, 0);
        }

        /// <summary>
        /// Binds the texture on the GPU..
        /// </summary>
        public void Bind()
        {
            GL.BindTexture(TextureTarget, TextureRef);
        }

        /// <summary>
        /// Gets or sets the gl texture reference.
        /// </summary>
        /// <value>
        /// The gl texture
        /// </value>
        public int TextureRef { get; protected set; }

        /// <summary>
        /// Gets or sets the texture target.
        /// </summary>
        /// <value>
        /// The texture target.
        /// </value>
        public TextureTarget TextureTarget { get; protected set;}

        /// <summary>
        /// Lookup for opengl pixel information.
        /// </summary>
        /// <value>
        internal static (PixelInternalFormat pif, OpenTK.Graphics.OpenGL.PixelFormat pi, PixelType pt) MapTextureFormat(SampleFormat format)
        {
            switch (format)
            {
                case SampleFormat.R8:
                    return (PixelInternalFormat.R8, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.UnsignedByte);
                case SampleFormat.R8I:
                    return (PixelInternalFormat.R8i, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.Byte);
                case SampleFormat.R8UI:
                    return (PixelInternalFormat.R8ui, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.UnsignedByte);
                case SampleFormat.R8_SNorm:
                    return (PixelInternalFormat.R8Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.Byte);

                case SampleFormat.R16:
                    return (PixelInternalFormat.R16, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.UnsignedShort);
                case SampleFormat.R16I:
                    return (PixelInternalFormat.R16i, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.Short);
                case SampleFormat.R16UI:
                    return (PixelInternalFormat.R16ui, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.UnsignedShort);
                case SampleFormat.R16_SNorm:
                    return (PixelInternalFormat.R16Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.Short);

                case SampleFormat.R32I:
                    return (PixelInternalFormat.R32i, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.Int);
                case SampleFormat.R32UI:
                    return (PixelInternalFormat.R32ui, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.UnsignedInt);
                case SampleFormat.R32F:
                    return (PixelInternalFormat.R32f, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.Float);

                case SampleFormat.RG8:
                    return (PixelInternalFormat.Rg8, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.UnsignedByte);
                case SampleFormat.RG8I:
                    return (PixelInternalFormat.Rg8i, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Byte);
                case SampleFormat.RG8UI:
                    return (PixelInternalFormat.Rg8ui, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.UnsignedByte);
                case SampleFormat.RG8_SNorm:
                    return (PixelInternalFormat.Rg8Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Byte);

                case SampleFormat.RG16:
                    return (PixelInternalFormat.Rg16, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.UnsignedShort);
                case SampleFormat.RG16I:
                    return (PixelInternalFormat.Rg16i, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Short);
                case SampleFormat.RG16UI:
                    return (PixelInternalFormat.Rg16ui, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.UnsignedShort);
                case SampleFormat.RG16_SNorm:
                    return (PixelInternalFormat.Rg16Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Short);
                case SampleFormat.RG16F:
                    return (PixelInternalFormat.Rg16f, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Float);

                case SampleFormat.RG32I:
                    return (PixelInternalFormat.Rg32i, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Int);
                case SampleFormat.RG32UI:
                    return (PixelInternalFormat.Rg32ui, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.UnsignedInt);
                case SampleFormat.RG32F:
                    return (PixelInternalFormat.Rg32f, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Float);


                case SampleFormat.RGB8:
                    return (PixelInternalFormat.Rgb8, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte);
                case SampleFormat.RGB8I:
                    return (PixelInternalFormat.Rgb8i, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Byte);
                case SampleFormat.RGB8UI:
                    return (PixelInternalFormat.Rgb8ui, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte);
                case SampleFormat.RGB8_SNorm:
                    return (PixelInternalFormat.Rgb8Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Byte);

                case SampleFormat.RGB16:
                    return (PixelInternalFormat.Rgb16, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Short);
                case SampleFormat.RGB16I:
                    return (PixelInternalFormat.Rgb16i, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Short);
                case SampleFormat.RGB16UI:
                    return (PixelInternalFormat.Rgb16ui, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte);
                case SampleFormat.RGB16_SNorm:
                    return (PixelInternalFormat.Rgb16Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Byte);
                case SampleFormat.RGB16F:
                    return (PixelInternalFormat.Rgb16f, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Float);

                case SampleFormat.RGB32I:
                    return (PixelInternalFormat.Rgb32i, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Int);
                case SampleFormat.RGB32UI:
                    return (PixelInternalFormat.Rgb32ui, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedInt);
                case SampleFormat.RGB32F:
                    return (PixelInternalFormat.Rgb32f, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Float);

                case SampleFormat.RGBA8:
                    return (PixelInternalFormat.Rgba8, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte);
                case SampleFormat.RGBA8_SNorm:
                    return (PixelInternalFormat.Rgb8Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Byte);
                case SampleFormat.RGBA8I:
                    return (PixelInternalFormat.Rgba8i, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Byte);
                case SampleFormat.RGBA8UI:
                    return (PixelInternalFormat.Rgba8ui, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte);                

                case SampleFormat.RGBA16:
                    return (PixelInternalFormat.Rgba16, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedShort);
                case SampleFormat.RGBA16_SNorm:
                    return (PixelInternalFormat.Rgba16Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Short);
                case SampleFormat.RGBA16I:
                    return (PixelInternalFormat.Rgba16i, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Short);
                case SampleFormat.RGBA16UI:
                    return (PixelInternalFormat.Rgba16ui, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedShort);
                case SampleFormat.RGBA16F:
                    return (PixelInternalFormat.Rgba16f, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float);

                case SampleFormat.RGBA32I:
                    return (PixelInternalFormat.Rgba32i, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Int);
                case SampleFormat.RGBA32UI:
                    return (PixelInternalFormat.Rgba32ui, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedInt);
                case SampleFormat.RGBA32F:
                    return (PixelInternalFormat.Rgba32f, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float);

                case SampleFormat.DepthComponent16:
                    return (PixelInternalFormat.DepthComponent16, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.UnsignedShort);
                case SampleFormat.DepthComponent24:
                    return (PixelInternalFormat.DepthComponent24, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.UnsignedInt);
                case SampleFormat.DepthComponent32:
                    return (PixelInternalFormat.DepthComponent32, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.UnsignedInt);
                case SampleFormat.DepthComponent32F:
                    return (PixelInternalFormat.DepthComponent32f, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.Float);

                case SampleFormat.Depth24_Stencil8:
                    return (PixelInternalFormat.Depth24Stencil8, OpenTK.Graphics.OpenGL.PixelFormat.DepthStencil, PixelType.UnsignedInt248);
                                
                default:
                    throw new Exception("Invalid Texture Format");
            }
        }

        #region Cleanup

        /// <summary>
        /// Deletes the underlying opengl buffer
        /// </summary>
        public void Destroy()
        {
            unsafe
            {
                GL.DeleteTexture(TextureRef);
            }
        }

        private bool _disposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
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
        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                // Fire off the event if people are attached to the event
                if (_OnDispose != null)
                    _OnDispose(this, new EventArgs());

                Destroy();

                _disposed = true;
            }
        }

        private event EventHandler _OnDispose;
        /// <summary>
        /// Occurs when [on dispose].
        /// </summary>
        public event EventHandler OnDispose
        {
            add
            {
                _OnDispose += value;
            }
            remove
            {
                _OnDispose -= value;
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="GLTexture"/> is reclaimed by garbage collection.
        /// </summary>
        ~GLTexture()
        {
            Dispose(false);
        }

        #endregion
    }
}
