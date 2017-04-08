using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Fe
{
    internal class GLTexture : IDisposable
    {
        public GLTexture()
        {

        }

        public void Create(TextureBase texture)
        {            
            switch (texture.TextureType)
            {
                case TextureType.Texture2D:
                    TextureTarget = TextureTarget.Texture2D;
                    break;
            }            

            TextureRef = GL.GenTexture();
            GL.BindTexture(TextureTarget, TextureRef);

            // Figure out our texture format data and throw an exception if we can't work it out.
            (var pif, var pf, var pt) = MapTextureFormat(texture.PixelFormat);

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

            GL.TexParameter(TextureTarget, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);

            // Stop working with this texture
            GL.BindTexture(TextureTarget, 0);
        }

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

        public TextureTarget TextureTarget { get; protected set;}

        /// <summary>
        /// Lookup for opengl pixel information.
        /// </summary>
        /// <value>
        internal static (PixelInternalFormat pif, OpenTK.Graphics.OpenGL.PixelFormat pi, PixelType pt) MapTextureFormat(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.R8:
                    return (PixelInternalFormat.R8, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.UnsignedByte);
                case PixelFormat.R8I:
                    return (PixelInternalFormat.R8i, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.Byte);
                case PixelFormat.R8UI:
                    return (PixelInternalFormat.R8ui, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.UnsignedByte);
                case PixelFormat.R8_SNorm:
                    return (PixelInternalFormat.R8Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.Byte);

                case PixelFormat.R16:
                    return (PixelInternalFormat.R16, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.UnsignedShort);
                case PixelFormat.R16I:
                    return (PixelInternalFormat.R16i, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.Short);
                case PixelFormat.R16UI:
                    return (PixelInternalFormat.R16ui, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.UnsignedShort);
                case PixelFormat.R16_SNorm:
                    return (PixelInternalFormat.R16Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.Short);

                case PixelFormat.R32I:
                    return (PixelInternalFormat.R32i, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.Int);
                case PixelFormat.R32UI:
                    return (PixelInternalFormat.R32ui, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.UnsignedInt);
                case PixelFormat.R32F:
                    return (PixelInternalFormat.R32f, OpenTK.Graphics.OpenGL.PixelFormat.Red, PixelType.Float);

                case PixelFormat.RG8:
                    return (PixelInternalFormat.Rg8, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.UnsignedByte);
                case PixelFormat.RG8I:
                    return (PixelInternalFormat.Rg8i, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Byte);
                case PixelFormat.RG8UI:
                    return (PixelInternalFormat.Rg8ui, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.UnsignedByte);
                case PixelFormat.RG8_SNorm:
                    return (PixelInternalFormat.Rg8Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Byte);

                case PixelFormat.RG16:
                    return (PixelInternalFormat.Rg16, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.UnsignedShort);
                case PixelFormat.RG16I:
                    return (PixelInternalFormat.Rg16i, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Short);
                case PixelFormat.RG16UI:
                    return (PixelInternalFormat.Rg16ui, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.UnsignedShort);
                case PixelFormat.RG16_SNorm:
                    return (PixelInternalFormat.Rg16Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Short);
                case PixelFormat.RG16F:
                    return (PixelInternalFormat.Rg16f, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Float);

                case PixelFormat.RG32I:
                    return (PixelInternalFormat.Rg32i, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Int);
                case PixelFormat.RG32UI:
                    return (PixelInternalFormat.Rg32ui, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.UnsignedInt);
                case PixelFormat.RG32F:
                    return (PixelInternalFormat.Rg32f, OpenTK.Graphics.OpenGL.PixelFormat.Rg, PixelType.Float);


                case PixelFormat.RGB8:
                    return (PixelInternalFormat.Rgb8, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte);
                case PixelFormat.RGB8I:
                    return (PixelInternalFormat.Rgb8i, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Byte);
                case PixelFormat.RGB8UI:
                    return (PixelInternalFormat.Rgb8ui, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte);
                case PixelFormat.RGB8_SNorm:
                    return (PixelInternalFormat.Rgb8Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Byte);

                case PixelFormat.RGB16:
                    return (PixelInternalFormat.Rgb16, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Short);
                case PixelFormat.RGB16I:
                    return (PixelInternalFormat.Rgb16i, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Short);
                case PixelFormat.RGB16UI:
                    return (PixelInternalFormat.Rgb16ui, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte);
                case PixelFormat.RGB16_SNorm:
                    return (PixelInternalFormat.Rgb16Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Byte);
                case PixelFormat.RGB16F:
                    return (PixelInternalFormat.Rgb16f, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Float);

                case PixelFormat.RGB32I:
                    return (PixelInternalFormat.Rgb32i, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Int);
                case PixelFormat.RGB32UI:
                    return (PixelInternalFormat.Rgb32ui, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedInt);
                case PixelFormat.RGB32F:
                    return (PixelInternalFormat.Rgb32f, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Float);

                case PixelFormat.RGBA8:
                    return (PixelInternalFormat.Rgba8, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte);
                case PixelFormat.RGBA8_SNorm:
                    return (PixelInternalFormat.Rgb8Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Byte);
                case PixelFormat.RGBA8I:
                    return (PixelInternalFormat.Rgba8i, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Byte);
                case PixelFormat.RGBA8UI:
                    return (PixelInternalFormat.Rgba8ui, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte);                

                case PixelFormat.RGBA16:
                    return (PixelInternalFormat.Rgba16, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedShort);
                case PixelFormat.RGBA16_SNorm:
                    return (PixelInternalFormat.Rgba16Snorm, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Short);
                case PixelFormat.RGBA16I:
                    return (PixelInternalFormat.Rgba16i, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Short);
                case PixelFormat.RGBA16UI:
                    return (PixelInternalFormat.Rgba16ui, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedShort);
                case PixelFormat.RGBA16F:
                    return (PixelInternalFormat.Rgba16f, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float);

                case PixelFormat.RGBA32I:
                    return (PixelInternalFormat.Rgba32i, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Int);
                case PixelFormat.RGBA32UI:
                    return (PixelInternalFormat.Rgba32ui, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedInt);
                case PixelFormat.RGBA32F:
                    return (PixelInternalFormat.Rgba32f, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float);

                case PixelFormat.DepthComponent16:
                    return (PixelInternalFormat.DepthComponent16, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.UnsignedShort);
                case PixelFormat.DepthComponent24:
                    return (PixelInternalFormat.DepthComponent24, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.UnsignedInt);
                case PixelFormat.DepthComponent32:
                    return (PixelInternalFormat.DepthComponent32, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.UnsignedInt);
                case PixelFormat.DepthComponent32F:
                    return (PixelInternalFormat.DepthComponent32f, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.Float);

                case PixelFormat.Depth24_Stencil8:
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
