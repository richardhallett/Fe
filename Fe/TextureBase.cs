using System;
using System.Collections.Generic;
using System.Text;

namespace Fe
{
    /// <summary>
    /// Interface to describe a generic texture object
    /// </summary>
    public abstract class TextureBase : GraphicsResource
    {
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }
        public abstract TextureFormat TextureFormat { get; set; }
        public abstract TextureType TextureType { get; set; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        internal abstract Object Data { get; }

        /// <summary>
        /// Gets the type of the texture data.
        /// </summary>
        /// <value>
        /// The type of the texture data.
        /// </value>
        internal abstract Type TextureDataType { get; set; }
    }
}
