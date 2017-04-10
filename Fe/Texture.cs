using System;
using System.Collections.Generic;
using System.Text;

namespace Fe
{
    /// <summary>
    /// Abstract class to describe a generic texture object
    /// </summary>
    public abstract class Texture : GraphicsResource
    {
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }
        public abstract SampleFormat SampleFormat { get; set; }
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
