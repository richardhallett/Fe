using System;
using System.Collections.Generic;
using System.Text;

namespace Fe
{
    /// <summary>
    /// Describes how a texture is sampled.
    /// </summary>
    public class TextureSampler : GraphicsResource
    {
        /// <summary>
        /// Gets or sets the mode to use when a u texture coordinate is outside the 0 to 1 range.
        /// </summary>
        public TextureAddressMode AddressU { get; set; } = TextureAddressMode.Repeat;

        /// <summary>
        /// Gets or sets the mode to use when a v texture coordinate is outside the 0 to 1 range.
        /// </summary>
        public TextureAddressMode AddressV { get; set; } = TextureAddressMode.Repeat;

        /// <summary>
        /// Gets or sets the mode to use when a w texture coordinate is outside the 0 to 1 range.
        /// </summary>
        public TextureAddressMode AddressW { get; set; } = TextureAddressMode.Repeat;

        /// <summary>
        /// Filter to use when the area of the fragment is larger than a texel.
        /// </summary>
        public TextureFilter MinFilter { get; set; } = TextureFilter.Linear;

        /// <summary>
        /// Filter to use when the area of the fragment is larger than a texel and whether it samples from multiple mipmaps.
        /// </summary>
        public TextureFilter MipFilter { get; set; } = TextureFilter.None;

        /// <summary>
        /// Filter to use when the area of the fragment is smaller than a texel.
        /// </summary>
        public TextureFilter MagFilter { get; set; } = TextureFilter.Linear;
    }
}
