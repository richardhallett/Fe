using System;
using System.Collections.Generic;
using System.Text;

namespace Fe
{
    /// <summary>
    /// How the texture sample will be filtered
    /// </summary>
    public enum TextureFilter
    {
        /// <summary>
        /// No Texture filtering if possible.
        /// </summary>
        None,
        /// <summary>
        /// Sample from the nearest texture coordinates "point sampling"
        /// </summary>
        Nearest,
        /// <summary>
        /// Sample from a weighted linear blend between adjacent samples.
        /// </summary>
        Linear,
    }
}
