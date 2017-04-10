using System;
using System.Collections.Generic;
using System.Text;

namespace Fe
{
    /// <summary>
    /// Describes the details of a te
    /// </summary>
    public class TextureStage
    {
        /// <summary>
        /// Texture to use for this stage
        /// </summary>
        public Texture Texture { get; set; }

        /// <summary>
        /// Uniform the texture will be bound to.
        /// </summary>
        public Uniform TextureUniform { get; set; }

        /// <summary>
        /// The sampler to use for this stage
        /// </summary>
        public TextureSampler TextureSampler { get; set; }
    }
}
