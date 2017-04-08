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
            (var pif, var pf, var pt) = MapTextureFormat(texture.TextureFormat);

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

            GL.TexParameter(TextureTarget, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

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
        internal static (PixelInternalFormat pif, PixelFormat pi, PixelType pt) MapTextureFormat(TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.R8:
                    return (PixelInternalFormat.R8, PixelFormat.Red, PixelType.UnsignedByte);
                case TextureFormat.R8I:
                    return (PixelInternalFormat.R8i, PixelFormat.Red, PixelType.Byte);
                case TextureFormat.R8UI:
                    return (PixelInternalFormat.R8ui, PixelFormat.Red, PixelType.UnsignedByte);
                case TextureFormat.R8_SNorm:
                    return (PixelInternalFormat.R8Snorm, PixelFormat.Red, PixelType.Byte);

                case TextureFormat.R16:
                    return (PixelInternalFormat.R16, PixelFormat.Red, PixelType.UnsignedShort);
                case TextureFormat.R16I:
                    return (PixelInternalFormat.R16i, PixelFormat.Red, PixelType.Short);
                case TextureFormat.R16UI:
                    return (PixelInternalFormat.R16ui, PixelFormat.Red, PixelType.UnsignedShort);
                case TextureFormat.R16_SNorm:
                    return (PixelInternalFormat.R16Snorm, PixelFormat.Red, PixelType.Short);

                case TextureFormat.R32I:
                    return (PixelInternalFormat.R32i, PixelFormat.Red, PixelType.Int);
                case TextureFormat.R32UI:
                    return (PixelInternalFormat.R32ui, PixelFormat.Red, PixelType.UnsignedInt);
                case TextureFormat.R32F:
                    return (PixelInternalFormat.R32f, PixelFormat.Red, PixelType.Float);

                case TextureFormat.RG8:
                    return (PixelInternalFormat.Rg8, PixelFormat.Rg, PixelType.UnsignedByte);
                case TextureFormat.RG8I:
                    return (PixelInternalFormat.Rg8i, PixelFormat.Rg, PixelType.Byte);
                case TextureFormat.RG8UI:
                    return (PixelInternalFormat.Rg8ui, PixelFormat.Rg, PixelType.UnsignedByte);
                case TextureFormat.RG8_SNorm:
                    return (PixelInternalFormat.Rg8Snorm, PixelFormat.Rg, PixelType.Byte);

                case TextureFormat.RG16:
                    return (PixelInternalFormat.Rg16, PixelFormat.Rg, PixelType.UnsignedShort);
                case TextureFormat.RG16I:
                    return (PixelInternalFormat.Rg16i, PixelFormat.Rg, PixelType.Short);
                case TextureFormat.RG16UI:
                    return (PixelInternalFormat.Rg16ui, PixelFormat.Rg, PixelType.UnsignedShort);
                case TextureFormat.RG16_SNorm:
                    return (PixelInternalFormat.Rg16Snorm, PixelFormat.Rg, PixelType.Short);
                case TextureFormat.RG16F:
                    return (PixelInternalFormat.Rg16f, PixelFormat.Rg, PixelType.Float);

                case TextureFormat.RG32I:
                    return (PixelInternalFormat.Rg32i, PixelFormat.Rg, PixelType.Int);
                case TextureFormat.RG32UI:
                    return (PixelInternalFormat.Rg32ui, PixelFormat.Rg, PixelType.UnsignedInt);
                case TextureFormat.RG32F:
                    return (PixelInternalFormat.Rg32f, PixelFormat.Rg, PixelType.Float);


                case TextureFormat.RGB8:
                    return (PixelInternalFormat.Rgb8, PixelFormat.Rgb, PixelType.UnsignedByte);
                case TextureFormat.RGB8I:
                    return (PixelInternalFormat.Rgb8i, PixelFormat.Rgb, PixelType.Byte);
                case TextureFormat.RGB8UI:
                    return (PixelInternalFormat.Rgb8ui, PixelFormat.Rgb, PixelType.UnsignedByte);
                case TextureFormat.RGB8_SNorm:
                    return (PixelInternalFormat.Rgb8Snorm, PixelFormat.Rgb, PixelType.Byte);

                case TextureFormat.RGB16:
                    return (PixelInternalFormat.Rgb16, PixelFormat.Rgb, PixelType.Short);
                case TextureFormat.RGB16I:
                    return (PixelInternalFormat.Rgb16i, PixelFormat.Rgb, PixelType.Short);
                case TextureFormat.RGB16UI:
                    return (PixelInternalFormat.Rgb16ui, PixelFormat.Rgb, PixelType.UnsignedByte);
                case TextureFormat.RGB16_SNorm:
                    return (PixelInternalFormat.Rgb16Snorm, PixelFormat.Rgb, PixelType.Byte);
                case TextureFormat.RGB16F:
                    return (PixelInternalFormat.Rgb16f, PixelFormat.Rgb, PixelType.Float);

                case TextureFormat.RGB32I:
                    return (PixelInternalFormat.Rgb32i, PixelFormat.Rgb, PixelType.Int);
                case TextureFormat.RGB32UI:
                    return (PixelInternalFormat.Rgb32ui, PixelFormat.Rgb, PixelType.UnsignedInt);
                case TextureFormat.RGB32F:
                    return (PixelInternalFormat.Rgb32f, PixelFormat.Rgb, PixelType.Float);

                case TextureFormat.RGBA8:
                    return (PixelInternalFormat.Rgba8, PixelFormat.Rgba, PixelType.UnsignedByte);
                case TextureFormat.RGBA8_SNorm:
                    return (PixelInternalFormat.Rgb8Snorm, PixelFormat.Rgba, PixelType.Byte);
                case TextureFormat.RGBA8I:
                    return (PixelInternalFormat.Rgba8i, PixelFormat.Rgba, PixelType.Byte);
                case TextureFormat.RGBA8UI:
                    return (PixelInternalFormat.Rgba8ui, PixelFormat.Rgba, PixelType.UnsignedByte);                

                case TextureFormat.RGBA16:
                    return (PixelInternalFormat.Rgba16, PixelFormat.Rgba, PixelType.UnsignedShort);
                case TextureFormat.RGBA16_SNorm:
                    return (PixelInternalFormat.Rgba16Snorm, PixelFormat.Rgba, PixelType.Short);
                case TextureFormat.RGBA16I:
                    return (PixelInternalFormat.Rgba16i, PixelFormat.Rgba, PixelType.Short);
                case TextureFormat.RGBA16UI:
                    return (PixelInternalFormat.Rgba16ui, PixelFormat.Rgba, PixelType.UnsignedShort);
                case TextureFormat.RGBA16F:
                    return (PixelInternalFormat.Rgba16f, PixelFormat.Rgba, PixelType.Float);

                case TextureFormat.RGBA32I:
                    return (PixelInternalFormat.Rgba32i, PixelFormat.Rgba, PixelType.Int);
                case TextureFormat.RGBA32UI:
                    return (PixelInternalFormat.Rgba32ui, PixelFormat.Rgba, PixelType.UnsignedInt);
                case TextureFormat.RGBA32F:
                    return (PixelInternalFormat.Rgba32f, PixelFormat.Rgba, PixelType.Float);

                case TextureFormat.DepthComponent16:
                    return (PixelInternalFormat.DepthComponent16, PixelFormat.DepthComponent, PixelType.UnsignedShort);
                case TextureFormat.DepthComponent24:
                    return (PixelInternalFormat.DepthComponent24, PixelFormat.DepthComponent, PixelType.UnsignedInt);
                case TextureFormat.DepthComponent32:
                    return (PixelInternalFormat.DepthComponent32, PixelFormat.DepthComponent, PixelType.UnsignedInt);
                case TextureFormat.DepthComponent32F:
                    return (PixelInternalFormat.DepthComponent32f, PixelFormat.DepthComponent, PixelType.Float);

                case TextureFormat.Depth24_Stencil8:
                    return (PixelInternalFormat.Depth24Stencil8, PixelFormat.DepthStencil, PixelType.UnsignedInt248);
                                
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
