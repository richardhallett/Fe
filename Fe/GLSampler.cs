using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Fe
{
    /// <summary>
    /// Represents an internal OpenGL Shader Object
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal class GLSampler : IDisposable
    {
        /// <summary>
        /// Creates the specified sampler.
        /// </summary>
        /// <param name="sampler">The sampler.</param>
        public void Create(TextureSampler sampler)
        {
            SamplerRef = GL.GenSampler();

            GL.SamplerParameter(SamplerRef, SamplerParameterName.TextureWrapS, (int)glTextureWrapMapping[sampler.AddressU]);
            GL.SamplerParameter(SamplerRef, SamplerParameterName.TextureWrapT, (int)glTextureWrapMapping[sampler.AddressV]);
            GL.SamplerParameter(SamplerRef, SamplerParameterName.TextureWrapR, (int)glTextureWrapMapping[sampler.AddressW]);

            // When we're working with mips we do special handling
            if (sampler.MipFilter != TextureFilter.None)
            {
                if (sampler.MipFilter == TextureFilter.Nearest && sampler.MinFilter == TextureFilter.Nearest)
                    GL.SamplerParameter(SamplerRef, SamplerParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapNearest);
                else if(sampler.MipFilter == TextureFilter.Nearest && sampler.MinFilter == TextureFilter.Linear)
                    GL.SamplerParameter(SamplerRef, SamplerParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapNearest);
                else if (sampler.MipFilter == TextureFilter.Linear && sampler.MinFilter == TextureFilter.Nearest)
                    GL.SamplerParameter(SamplerRef, SamplerParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapNearest);
                else
                    GL.SamplerParameter(SamplerRef, SamplerParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            } else
            {
                if (sampler.MinFilter == TextureFilter.Nearest)
                    GL.SamplerParameter(SamplerRef, SamplerParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                else
                    GL.SamplerParameter(SamplerRef, SamplerParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

                if (sampler.MagFilter == TextureFilter.Nearest)
                    GL.SamplerParameter(SamplerRef, SamplerParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                else
                    GL.SamplerParameter(SamplerRef, SamplerParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            }

            // TODO: Support for antrioscopic filtering.
        }

        /// <summary>
        /// Gets or sets the gl sampler reference.
        /// </summary>
        /// <value>
        /// The gl texture
        /// </value>
        public int SamplerRef { get; protected set; }

        /// <summary>
        /// Lookup for texture address to texture wrap
        /// </summary>
        internal static Dictionary<TextureAddressMode, TextureWrapMode> glTextureWrapMapping = new Dictionary<TextureAddressMode, TextureWrapMode>()
        {
            {TextureAddressMode.Repeat, TextureWrapMode.Repeat},
            {TextureAddressMode.Border, TextureWrapMode.ClampToBorder},
            {TextureAddressMode.Clamp, TextureWrapMode.Clamp},
            {TextureAddressMode.Mirror, TextureWrapMode.MirroredRepeat},
        };

        #region Cleanup

        /// <summary>
        /// Deletes the underlying opengl object
        /// </summary>
        public void Destroy()
        {
            unsafe
            {
                GL.DeleteSampler(SamplerRef);
            }
        }
        
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {

                disposedValue = true;
            }
        }

        ~GLSampler()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);            
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
